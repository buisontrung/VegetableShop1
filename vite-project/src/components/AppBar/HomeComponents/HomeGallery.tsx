import Slider from 'react-slick';
import './HomeGallery.scss'
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

const HomeGallery = () => {
    

    const settings = {
        dots: true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        speed: 2000,
        autoplaySpeed: 2000,
        cssEase: "linear"
      };
    return (
        <div className='home__gallery'>
            <div className="home-gallery">
                <div className="container">
                    <div className="row">
                    <div className="hg-slider__container">
      <Slider {...settings}>
        <div>
          <img src="hg_1.jpg" alt="" />
        </div>
        <div>
        <img src="hg_2.jpg" alt="" />
        </div>
        <div>
        <img src="hg_4.jpg" alt="" />
        </div>
        <div>
        <img src="hg_1.jpg" alt="" />
        </div>
        
      </Slider>
    </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default HomeGallery
