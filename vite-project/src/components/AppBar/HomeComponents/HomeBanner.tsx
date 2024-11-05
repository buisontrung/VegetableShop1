
import './HomeBanner.scss'
const HomeBanner = () => {
    return (
        <div className='home__banner'>
            <img src="homebanner1.jpg" alt="" />
            <div className='container'>
                <div className="row">
                    <div className="p-4 d-flex justify-content-center text-center">
                        <div className="row justify-content-center flex-column align-items-center">
                            <div className="banner-text">
                                <div className="banner-text-up">
                                    Thực Phẩm Tốt
                                </div>
                                <div className="banner-text-bold">
                                    Cảm Xúc Vui
                                </div>
                                <div className="banner-text-down">
                                Thực phẩm của chúng tôi luôn tươi mới, không chất độc hại
                                </div>
                            </div>
                            <a href="" className='btnHome'>XEM NGAY</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default HomeBanner
