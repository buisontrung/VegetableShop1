import { Link } from 'react-router-dom'
import './Footer.scss'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faFacebook, faGoogle, faInstagram, faTwitter, faYoutube } from '@fortawesome/free-brands-svg-icons'
const Footer = () => {
    return (
        <footer id='footer'>
            <div className="container">
                <div className="row pt-5">
                    <div className="footer-logo text-center">
                        <Link to='#'>
                            <img src="logo_11.png" alt="" />
                        </Link>
                        <div className="ft-social-network">
                            <Link to='#' className='fb-icon'>
                                <FontAwesomeIcon icon={faFacebook} />
                            </Link>
                            <Link to='#' className='ins-icon'>
                                <FontAwesomeIcon icon={faInstagram} />
                            </Link>
                            <Link to='#' className='yt-icon'>
                                <FontAwesomeIcon icon={faYoutube} />
                            </Link>
                            <Link to='#' className='tt-icon'>
                                <FontAwesomeIcon icon={faTwitter} />
                            </Link>
                            <Link to='#' className='gg-icon'>
                                <FontAwesomeIcon icon={faGoogle} />
                            </Link>
                        </div>
                    </div>
                    <div className="footer-main">
                        <div className="row">
                            <div className='col-12 col-md-3'>
                                <div className="footer-info">
                                    <h3>LIÊN HỆ</h3>
                                    <ul>
                                        <li>179, đường Võ Văn Kiệt, P An Thới, Q Bình Thủy, TP Cần Thơ</li>
                                    </ul>
                                </div>
                            </div>
                            <div className='col-12 col-md-3'>
                                <h3>HỖ TRỢ KHÁCH HÀNG</h3>
                                <ul>
                                    <li><Link to="#">Kinh nghiệm mua hàng</Link></li>
                                    <li><Link to="#">HƯỚNG DẪN MUA HÀNG</Link></li>
                                    <li><Link to="#">Chính sách đổi trả </Link></li>
                                    <li><Link to="#">Hình thức thanh toán</Link></li>
                                </ul>
                            </div>
                            <div className='col-12 col-md-3'>
                                <h3>LIÊN KẾT NHANH
                                </h3>
                                <ul>
                                    <li><Link to="#">Trang chủ</Link></li>
                                    <li><Link to="#">Hình thức thanh toán</Link></li>
                                    <li><Link to="#">Chính sách đổi trả </Link></li>
                                    <li><Link to="#">Kinh nghiệm mua hàng</Link></li>
                                </ul>
                            </div>
                            <div className='col-12 col-md-auto'>
                                <h3 >GALLERY
                                </h3>
                                <div className='gallery'>
                                    <div className='gallery-item'><img src="ms_banner_img1.jpg" alt="" /></div>
                                    <div className='gallery-item'><img src="ms_banner_img2.jpg" alt="" /></div>
                                    <div className='gallery-item'><img src="ms_banner_img3.jpg" alt="" /></div>
                                    <div className='gallery-item'><img src="ms_banner_img1.jpg" alt="" /></div>
                                    <div className='gallery-item'><img src="ms_banner_img2.jpg" alt="" /></div>
                                    <div className='gallery-item'><img src="ms_banner_img3.jpg" alt="" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    )
}

export default Footer
