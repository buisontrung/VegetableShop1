import axios from "axios";
import { createContext, ReactNode, useEffect, useState } from "react";
import { useCookies } from "react-cookie";
import { APIENDPOINT } from "../../utils/constant";
import { user } from "../../utils/IVegetable";

interface AuthContextType {
    isAuthenticated: boolean;
    login: (authData: { token: string, refreshToken: string }) => void;
    logout: () => void;
    user: user| null;
}
const AuthContext = createContext<AuthContextType | undefined>(undefined);


interface AuthProviderProps {
    children: ReactNode;
}
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [currentUser, setCurrentUser] = useState<user | null>(null);
    const [cookies, setCookie, removeCookie] = useCookies(['accessToken', 'refreshToken']);
    useEffect(() => {
        const handleTokenRefresh = async () => {
            const rftoken = cookies.refreshToken;
            const accessToken = cookies.accessToken;

            if (rftoken) {
                setIsAuthenticated(true); // Tạm thời đặt xác thực thành true

                if (!accessToken) {
                    try {
                        const response = await axios.post(`${APIENDPOINT}/Auth/api/Auth/GenerateToken?refreshToken=${rftoken}`);
                        const newAccessToken = response.data;
                        setCookie('accessToken', newAccessToken, { path: '/', maxAge: 60 });
                    } catch (error) {
                        console.error('Error generating token:', error);
                        setIsAuthenticated(false);
                    }
                }

                // Lấy thông tin người dùng từ sessionStorage nếu có, tránh gọi API không cần thiết
                const savedUser = sessionStorage.getItem("user");
                if (savedUser) {
                    setCurrentUser(JSON.parse(savedUser));
                } else {
                    // Nếu không có, gọi API lấy thông tin người dùng và lưu vào sessionStorage
                    try {
                        const response = await axios.get(`${APIENDPOINT}/auth/api/Auth/getuser`, {
                            headers: {
                                Authorization: `Bearer ${accessToken}`,
                            },
                        });
                        setCurrentUser(response.data);
                        sessionStorage.setItem("user", JSON.stringify(response.data));
                    } catch (error) {
                        console.log("Error fetching user data:", error);
                    }
                }
            } else {
                setIsAuthenticated(false);
            }
        };

        handleTokenRefresh();
    }, [cookies.refreshToken, cookies.accessToken, setCookie]);
    const login = ({ token, refreshToken }: { token: string, refreshToken: string }) => {
        // Lưu token vào localStorage
         
        
        // Lưu cả token và refreshToken vào cookie
        setCookie('accessToken', token, { path: '/', maxAge: 60 });
        setCookie('refreshToken', refreshToken, { path: '/', maxAge: 604800 }); 
    
        // Đặt trạng thái xác thực
        setIsAuthenticated(true);
    };
    const logout = () => {
        // Xóa token từ cookies và localStorage
        const rftoken = cookies.refreshToken;
        axios.put(`${APIENDPOINT}/Auth/api/Auth/RevokedToken?refreshToken=${rftoken}`)
        removeCookie('accessToken');
        removeCookie('refreshToken');
        sessionStorage.removeItem("user")
        
        // Đặt trạng thái đăng xuất
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, login, logout,user:currentUser, }}>
            {children}
        </AuthContext.Provider>
    )
}
export default AuthContext;