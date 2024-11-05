
import Slider from "react-slick";
import "slick-carousel/slick/slick.css"; 
import "slick-carousel/slick/slick-theme.css";
import "./SlickSlider.scss"
const SlickSlider = () => {
    const settings = {
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        speed: 1000,
        autoplaySpeed: 4000,
        cssEase: "linear"
      };
      return (
        
          <div className="slider-container">
          <Slider {...settings}>
            <div>
              <img className="slick-item" src="banner1.jpg"  alt="" />
            </div>
            <div>
              <img className="slick-item" src="banner2.jpg" alt="" />
            </div>
            <div>
              <img className="slick-item" src="slickslider2.png" alt="" />
            </div>
           
          </Slider>
        </div>
        
      );
}

export default SlickSlider
