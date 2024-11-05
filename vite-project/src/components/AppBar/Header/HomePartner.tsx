import Slider from "react-slick";
import { homepartner } from "../../../utils/array";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import './HomePartner.scss'
const HomePartner = () => {
    const settings = {
        dots: true,
        infinite: true,
        slidesToShow: 5,
        slidesToScroll: 1,
        autoplay: true,
        speed: 2500,
        autoplaySpeed: 2000,
        cssEase: "linear"
      };
  return (
    <div className="home_partner">
      <div className="container">
        <div className="row text-center">
            <div className="section-title">
                <h2 className="display-4 fw-normal">Đối Tác</h2>
            </div>
            <Slider {...settings}>
      {
        homepartner.map((logo,index)=>(
            
                <div key={index}><img key={index} src={logo} alt={`Partner logo ${index + 1}`}  /></div>
            
        ))
      }
        </Slider>
        </div>
      </div>
    </div>
  )
}

export default HomePartner
