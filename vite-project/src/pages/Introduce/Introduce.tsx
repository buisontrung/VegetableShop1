import { useState } from "react";
import Header from "../../components/AppBar/Header/Header"
import NavDrawer from "../../components/AppBar/NavDrawer/NavDrawer"

import Footer from "../../components/AppBar/Footer/Footer";
import BackToTop from "../../components/AppBar/BackTop/BackToTop";
import Contact from "../../components/AppBar/ContactIcon/ContactButton";
import BreadCrumb from "../../components/AppBar/BreadCrumb/BreadCrumb";
import BlogWrapper from "../../components/AppBar/IntroduceComponents/BlogWrapper";
import IntroduceWrapper from "../../components/AppBar/IntroduceComponents/IntroduceWrapper";
import CategoryTable from "../../components/AppBar/CategoryTable/CategoryTable";


const Introduce = () => {
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

                    <IntroduceWrapper />
                    <CategoryTable />
                </BlogWrapper>
            </div>
            <Footer />
            <BackToTop />
            <Contact />
        </>
    )
}

export default Introduce
