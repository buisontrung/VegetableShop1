import './HomeAbout.scss'

const HomeAbout = () => {
    return (
        <div className='home__about'>
            <div className="pt-5 mt-5">
                <div className="container">
                    <div className="row">
                        <div className="section-title">
                            <h2 className='fw-normal text-center mb-5 display-2'>Nông Sản Xanh</h2>
                        </div>
                        <div className='section-about'>

                            <div className="row">
                                <div className="col-sm-3 text-center">
                                    <div className="about-item">
                                        <div className="about-icon">
                                            <div className="about-circle">
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <img src="hab_left_icon_1.png" alt="" />
                                            </div>

                                        </div>
                                        <div className="about-content">
                                            <div className="about-title">
                                                Hàng hóa tươi mới, đa dạng
                                            </div>
                                            <div className="about-desc">Cung cấp tốt nhất nông sản tươi mới và đa dạng nhất 
                                                để đáp ứng nhu cầu khác nhau của khách hàng. </div>
                                        </div>
                                    </div>
                                    <div className="about-item">
                                        <div className="about-icon">
                                            <div className="about-circle">
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <img src="hab_left_icon_2.png" alt="" />
                                            </div>

                                        </div>
                                        <div className="about-content">
                                            <div className="about-title">
                                            Giao hàng nhanh
                                            </div>
                                            <div className="about-desc">Cung cấp dịch vụ giao hàng nhanh và hiệu quả để khách hàng
                                                 có thể nhận sản phẩm trong thời gian ngắn nhất. </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-sm-6">

                                </div>
                                <div className="col-sm-3 text-center">
                                    <div className="about-item">
                                        <div className="about-icon">
                                            <div className="about-circle">
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <img src="hab_right_icon_1.png" alt="" />
                                            </div>

                                        </div>
                                        <div className="about-content">
                                            <div className="about-title">
                                            Chuyên nghiệp và tận tình
                                            </div>
                                            <div className="about-desc">Nhân viên chuyên nghiệp, có kinh nghiệm và 
                                                có đủ kiến thức để hỗ trợ khách hàng </div>
                                        </div>
                                    </div>
                                    <div className="about-item">
                                        <div className="about-icon">
                                            <div className="about-circle">
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <div className="circle"></div>
                                                <img src="hab_right_icon_2.png" alt="" />
                                            </div>

                                        </div>
                                        <div className="about-content">
                                            <div className="about-title">
                                            Giá thành hợp lý
                                            </div>
                                            <div className="about-desc">Cung cấp giá cả hợp lý với chất lượng tốt nhất để khách hàng sẽ 
                                                được tối ưu tài chính cũng như có được sự tin tưởng từ khách hàng. </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default HomeAbout
