import { createContext} from "react";



type CartOverlayContextType = {
    isCartOpen: boolean,
    openCart: ()=> void,
    closeCart: ()=>void,
}
const CartOverlayContext = createContext< CartOverlayContextType>({
    isCartOpen:true,
    openCart: () => {},
    closeCart: () => {}, 
});

export default CartOverlayContext;