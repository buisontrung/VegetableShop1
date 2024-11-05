import { useContext } from "react";
import CartOverlayContext from "./CartOverlayContext";
import { ShoppingCartContext } from "./ShoppingCartContext";

export const useCartOverlay = () =>{
    {
        const context = useContext(CartOverlayContext);
        if (!context) {
            throw new Error('useAuth must be used within an AuthProvider');
        }
        return context;
        }
}
export const useCart = () =>{
    const context = useContext(ShoppingCartContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
    }