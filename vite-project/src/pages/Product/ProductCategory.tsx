import { useState } from "react";
import NavDrawer from "../../components/AppBar/NavDrawer/NavDrawer";
import Header from "../../components/AppBar/Header/Header";
import BreadCrumb from "../../components/AppBar/BreadCrumb/BreadCrumb";
import BlogWrapper from "../../components/AppBar/IntroduceComponents/BlogWrapper";
import CategoryTable from "../../components/AppBar/CategoryTable/CategoryTable";
import Footer from "../../components/AppBar/Footer/Footer";
import BackToTop from "../../components/AppBar/BackTop/BackToTop";
import Contact from "../../components/AppBar/ContactIcon/ContactButton";
import Products from "../../components/AppBar/Product/Products";


const ProductCategory = () => {
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
                <BlogWrapper>
                <CategoryTable />
                <Products/>
                    
                </BlogWrapper>
            </div>
            <Footer />
            <BackToTop />
            <Contact />
        </>
    )
}

export default ProductCategory
