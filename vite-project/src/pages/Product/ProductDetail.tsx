import  {  useState } from 'react'
import NavDrawer from '../../components/AppBar/NavDrawer/NavDrawer'
import Header from '../../components/AppBar/Header/Header'
import BreadCrumb from '../../components/AppBar/BreadCrumb/BreadCrumb'
import Footer from '../../components/AppBar/Footer/Footer'
import BackToTop from '../../components/AppBar/BackTop/BackToTop'
import Contact from '../../components/AppBar/ContactIcon/ContactButton'

import Product1 from '../../components/AppBar/Product/Product'
import { useCart } from '../../components/Context/useCartOverlay'


const ProductDetail = () => {
  const { cartItems } = useCart();
  
    console.log('Cart items updated', cartItems);

    const [isDrawerOpen, setIsDrawerOpen] = useState(false);
    

    const toggleDrawer = () => {
        setIsDrawerOpen(!isDrawerOpen);
    };
    
  return (
    <>
            <NavDrawer isOpen={isDrawerOpen} onClose={() => setIsDrawerOpen(false)} />
            <Header onMenuClick={toggleDrawer} />
            <BreadCrumb />
            <div className="page-content">
                <Product1 />
            </div>
            <Footer />
            <BackToTop />
            <Contact />
        </>
  )
}

export default ProductDetail
