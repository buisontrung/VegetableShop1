import { createContext, useState } from "react";
import { Product } from "../../utils/IVegetable";
import CartOverlayContext from "./CartOverlayContext";

// Định nghĩa CartItem và ShoppingCartContextType
export type CartItem = {
  product: Product;
  quantity: number;
};

type ShoppingCartContextType = {
  cartItems: CartItem[];
  addToCart: (product: Product, quantity: number) => void;
  removeFromCart: (productId: number) => void;
  updateCartItemQuantity: (productId: number, quantity: number) => void;
  clearCart: () => void;
};

// Tạo context và export theo cách thông thường
export const ShoppingCartContext = createContext<ShoppingCartContextType| undefined>(undefined);

type Props = {
  children: React.ReactNode;
  storeKey: string;
};

// ShoppingCartProvider là export mặc định
const ShoppingCartProvider: React.FC<Props> = ({ children, storeKey }) => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [isCartOpen, setIsCartOpen] = useState(false);


  // Cart management functions
  const addToCart = (product: Product, quantity: number) => {
    setCartItems((prevCartItems) => {
        const existingItem = prevCartItems.find(
            (item) => item.product.id === product.id
        );

        let updatedCartItems;
        
        if (existingItem) {
            updatedCartItems = prevCartItems.map((item) =>
                item.product.id === product.id
                    ? { ...item, quantity: item.quantity + quantity }
                    : item
            );
        } else {
            updatedCartItems = [...prevCartItems, { product, quantity }];
        }

        // Lưu cartItems vào Local Storage ngay sau khi đã cập nhật
        localStorage.setItem('cartItems', JSON.stringify(updatedCartItems));
        return updatedCartItems; // Trả về cartItems đã cập nhật
    });

    openCart();
};


  const removeFromCart = (productId: number) => {
    setCartItems(
      cartItems.filter((item) => item.product.id !== productId)
    );
  };

  const updateCartItemQuantity = (productId: number, quantity: number) => {
    const existingCartItemIndex = cartItems.findIndex(
      (item) => item.product.id === productId
    );

    if (existingCartItemIndex >= 0) {
      const updatedCartItems = [...cartItems];
      updatedCartItems[existingCartItemIndex].quantity = quantity;
      setCartItems(updatedCartItems);
    }
  };

  const clearCart = () => {
  
    setCartItems([]);
    localStorage.removeItem(storeKey)
    console.log(100);
  };

  const openCart = () => {
    setIsCartOpen(true);
  };

  const closeCart = () => {
    setIsCartOpen(false);
  };




  return (
    <ShoppingCartContext.Provider value={{ cartItems, addToCart, removeFromCart, updateCartItemQuantity, clearCart }}>
      <CartOverlayContext.Provider value={{isCartOpen,openCart,closeCart}}>
        {children}
      </CartOverlayContext.Provider>
    </ShoppingCartContext.Provider>
  );
};

export default ShoppingCartProvider;
