import { Link } from 'react-router-dom'
import './HomeFeature.scss'
const HomeFeature = () => {
  return (
    <div className="home__feature">
      <div className="container">
        <div className="row text-center">
            <div className="section-title display-2 fw-normal">
            Sản Phẩm Nổi Bật
            </div>
            <div className="section-content">
                
                <div className="row text-start">
                    <div className="col-7">
                        <div className="product-image">
                        <img src="hotproduct.png" alt="" />
                        </div>
                    </div>
                    <div className="col-5 ps-5">
                        <div className="product-info">
                            <div className="product-title">
                                MỒNG TƠI
                            </div>
                            <div className="product-review">
                                <div className="product-price">
                                    <div className="current-price">
                                        19000 VNĐ
                                    </div>
                                </div>
                            </div>
                            <div className="product-desc">
                                <ol>
                                    <li>Tên gọi: mồng tơi, mùng tơi, lạc quỳ</li>
                                    <li>Công dụng: thanh nhiệt, giải độc, nhuận tràng, giảm đau thông tiện, ngăn ngừa loãng xương, hỗ trợ dưỡng da, bồi bổ cho phụ nữ có thai, tăng sữa cho sản phụ, tốt cho mắt...</li>
                                </ol>
                            </div>
                            <div className="product-action">
                                <Link to="#">
                                <button>CHI TIẾT</button>
                              </Link>
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

export default HomeFeature
