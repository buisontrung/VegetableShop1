import { useState } from "react";
import Header from "../../components/AppBar/Header/Header"
import NavDrawer from "../../components/AppBar/NavDrawer/NavDrawer"
import SlickSlider from "../../components/AppBar/SlickSlider/SlickSlider";
import HomeAbout from "../../components/AppBar/HomeComponents/HomeAbout";
import HomeBanner from "../../components/AppBar/HomeComponents/HomeBanner";
import HomeProduct from "../../components/AppBar/HomeComponents/HomeProduct";
import HomeFeature from "../../components/AppBar/HomeComponents/HomeFeature";
import HomeGallery from "../../components/AppBar/HomeComponents/HomeGallery";
import HomeClient from "../../components/AppBar/HomeComponents/HomeClient";
import HomePartner from "../../components/AppBar/Header/HomePartner";
import Footer from "../../components/AppBar/Footer/Footer";
import BackToTop from "../../components/AppBar/BackTop/BackToTop";
import Contact from "../../components/AppBar/ContactIcon/ContactButton";


const Home = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);

  const toggleDrawer = () => {
    setIsDrawerOpen(!isDrawerOpen);
  };
  return (
    <>
      <NavDrawer isOpen={isDrawerOpen} onClose={() => setIsDrawerOpen(false)} />
      <Header onMenuClick={toggleDrawer} />
      <div className="page-content">
        <SlickSlider />
        <main className="home-sb">
          <HomeAbout />
          <HomeBanner />
          <HomeProduct />
          <HomeFeature />
          <HomeGallery />
          <HomeClient />
          <HomePartner />
        </main>
        
      </div>
      <Footer/>
      <BackToTop/>
      <Contact/>
    </>
  )
}

export default Home
