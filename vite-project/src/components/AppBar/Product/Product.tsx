import axios from "axios"
import {  useEffect, useState } from "react"
import { APIENDPOINT } from "../../../utils/constant"
import { Product, ShoppingCartItem } from "../../../utils/IVegetable";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar } from "@fortawesome/free-solid-svg-icons";
import Slider from "react-slick";
import { Link, useParams } from "react-router-dom";
// import { useCart } from "../../Context/useCartOverlay";
import { useAuth } from "../../Context/useAuth";






const Product1 = () => {
    
// const { addToCart} = useCart()
    const [product, setProduct] = useState<Product>();
    const {user} = useAuth();
    const [recommend, setRecommend] = useState<Product[]>([]);
    const [choose,setChoose] = useState(1);
    const [tab, setTab] = useState(1);
    const params = useParams<{ id: string |undefined }>();
    const [quantity, setQuantity] = useState(1);
    const [cartItem, setCartItem] = useState<ShoppingCartItem|undefined>();
    const price = product?.productVariantDTOs
    .find(varian => varian.id === choose)?.unitPrice; 
    const totalQuantity = product?.productVariantDTOs
    .filter(varian => varian.id === choose) // Lọc biến thể có id bằng với choose
    .flatMap(varian => varian.productInventorySuppliers) // Lấy danh sách các nhà cung cấp từ biến thể
    .reduce((total, supplier) => total + supplier.quantity, 0) || 0; // Tính tổng số lượng
    useEffect(()=>{
        axios.get(`${APIENDPOINT}/ShoppingCart/api/ShoppingCart/user=${user?.id}&productVarianId=${choose}`)
        .then(res=>{
            setCartItem(res.data)
            
        }).catch(err=>{
            console.log(err)
        })
    },[choose,user?.id])
    const handleAddtoCart = () => {
        if (choose === undefined || !product?.productVariantDTOs[choose]) {
            console.error("No variant selected or variant not found.");
            return;
        }
        
        if ((cartItem?.quantity ?? 0) + quantity > totalQuantity) {
            alert("Không đủ số lượng");
        } else {
            axios.post(`${APIENDPOINT}/ShoppingCart/api/ShoppingCart/add`, {
                id: 0,
                userId: user?.id,
                quantity: quantity,
                price: product.productVariantDTOs[choose].unitPrice, // Ensure this accesses the actual price property
            })
            .then(res => {
                console.log("Item added to cart:", res.data);
            })
            .catch(err => {
                console.error("Error adding item to cart:", err);
            });
            alert("Thành công");

        }
        
    };
    
    const settings = {
        customPaging: function (i: number) {
            return (
                <a>
                    <img src={product?.productImageDTOs[i].imageUrl} style={{ width: '100%' }} alt={`Thumbnail ${i + 1}`} />

                </a>
            );
        },
        dots: true,
        dotsClass: "slick-dots slick-thumb",
        infinite: true,
        speed: 500,
        slidesToShow: 1,
        slidesToScroll: 1
    };
    useEffect(() => {
        axios.get(`${APIENDPOINT}/product/api/Product/id=${params.id}`)
            .then(res => {
                setProduct(res.data)
            })
            .catch(err => {
                console.log(err);
            })
            
            
    }, [params.id])
    useEffect(() => {
        const fetchCategoryId = async () => {
            if (!product?.productCategoryId) {

                return; 
            }

            try {
                const response = await axios.get(`${APIENDPOINT}/product/api/Product/productcategoryid=${product.productCategoryId}`);
                setRecommend(response.data);
            } catch (error) {
                console.error("Error fetching category ID:", error);
            }
        };

        fetchCategoryId();
    }, [product?.productCategoryId]);
    const handleTab = (key: number) => {
        setTab(key)
    }
    const handleQuantityChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = e.target.value;
        // Kiểm tra để chỉ nhận các giá trị là số
        
        if (/^\d*$/.test(newValue)) {
            setQuantity(newValue === "" ? 1 : parseInt(newValue, 10));
            
          }
        if(parseInt(newValue, 10)> totalQuantity){
                setQuantity(totalQuantity)
        }
      };
      
    return (
        <div className="pt-5 container">
            <nav><Link to="#">Trang chủ</Link>/ <Link to="/danh-muc-san-pham">Sản phẩm</Link>  / {product?.productCategoryDTO?.productCategoryName}</nav>
            <div className="container pt-5 pb-3 mb-5">

                <div className="row">
                    <div className="col-12 col-md-5 mx-auto">
                        {product?.productImageDTOs && product.productImageDTOs.length > 1 ? (
                            <Slider {...settings}>
                                {product.productImageDTOs.map((image, index) => (
                                    <div key={index}>
                                        <img src={image.imageUrl} style={{ width: "100%", border: "1px solid #d3ced2" }} alt={`Product image ${index + 1}`} />
                                    </div>
                                ))}
                            </Slider>
                        ) : (
                            <img src={product?.imageUrl&&`${APIENDPOINT}/product/images/${product?.imageUrl}`} style={{ width: "100%", height: "auto", border: "1px solid #d3ced2" }} alt="" />
                        )}
                    </div>
                    <div className="col-12 col-md-6 mx-auto">
                        <div className="product-content">
                            <div className="pro-header">
                                <h1>{product?.productName}</h1>
                            </div>

                            <div className="pro-detail">

                                <p className="fs-6"><strong>Danh Mục</strong>: {product?.productCategoryDTO?.productCategoryName}</p>
                            </div>
                            <div className="pro-price pb-3">
                                <span className="current-price fs-5" style={{ color: "#86bc42" }}> {price} VNĐ</span>
                            </div>
                            <div >
                                <div className="swatch d-flex mb-4">
                                    <div className="me-4"><h4 className="fw-bold mb-0 mt-1">QUY CÁCH</h4></div>
                                    <div className="select-wrap d-flex">
                                        
                                            {product?.productVariantDTOs.map(pVarian =>{
                                                return (<div  className="n-sd swatch-element mx-2">
                                                
                                                <input className="variant-0 input-product d-none"  type="radio" name="option1" value="Hộp-250gr" checked={undefined} />
                                            <label htmlFor="" className={pVarian.id == choose?"sd":""} onClick={()=>{setChoose(pVarian.id)}}>
                                                {pVarian.variantName}
                                                <img className="crossed-out" src="//theme.hstatic.net/1000302988/1000808903/14/soldout.png?v=161" />
                                                <img className={`img-check ${pVarian.id == choose?"":"d-none"}`} src="//theme.hstatic.net/1000302988/1000808903/14/select-pro.png?v=161" />
                                            </label>
                                            </div>  )
                                            })}
                                            
                                        
                                    </div>
                                </div>
                                <div className="quantity-product">
                                    <div className="me-4">
                                        <h4 className="fw-bold mb-0 mt-1">SỐ LƯỢNG</h4>
                                    </div>
                                    <div className="js-qty">
                                        {/* Nút giảm số lượng */}
                                        <button
                                            type="button"
                                            className="js-qty__adjust js-qty__adjust--minus icon-fallback-text"
                                            data-id="Quantity"
                                            onClick={() => setQuantity(quantity > 1 ? quantity - 1 : 1)} // Không cho giảm dưới 1
                                        >
                                            <span className="icon icon-minus" aria-hidden="true"></span>
                                            <span className="fallback-text" aria-hidden="true">−</span>
                                            <span className="visually-hidden">Giảm số lượng sản phẩm đi 1</span>
                                        </button>

                                        {/* Ô nhập số lượng */}
                                        <input
                                        type="text"
                                        className="js-qty__num"
                                        value={quantity}
                                        
                                        
                                        
                                        name="quantity"
                                        id="Quantity"
                                        onChange={handleQuantityChange} // Sự kiện onChange để cập nhật state
                                        />

                                        {/* Nút tăng số lượng */}
                                        <button
                                            type="button"
                                            className="js-qty__adjust js-qty__adjust--plus icon-fallback-text"
                                            data-id="Quantity"
                                            onClick={() => quantity<totalQuantity? setQuantity(quantity + 1): setQuantity(totalQuantity)}
                                        >
                                            <span className="icon icon-plus" aria-hidden="true"></span>
                                            <span className="fallback-text" aria-hidden="true">+</span>
                                            <span className="visually-hidden">Tăng số lượng sản phẩm lên 1</span>
                                        </button>
                                    </div>
                                    
                                    <div className="qty ms-3">
                                    <p className="m-0">
                                        Số lượng : {totalQuantity}
                                    </p>
                                    
                                    </div>
                                    
                                </div>
                                <div className="mt-4">
                                        <div className="d-flex">
                                        <button type="button" className="btn btn-tinted btn--l YuENex a_JvBi me-4" aria-disabled="false"
                                         onClick={handleAddtoCart}
                                        ><img alt="icon-add-to-cart" className="_kL9Hf" src="https://deo.shopeemobile.com/shopee/shopee-pcmall-live-sg/productdetailspage/b96050554b3be4feea08.svg"
                                       
                                        />
                                        <span>Thêm Vào Giỏ Hàng</span>
                                        </button>
                                        <button type="button" className="btn btn-solid-primary btn--l YuENex eFAm_w" aria-disabled="false">
                                            <div>Mua Với Voucher</div><div className="Rt4WYl">{product?.price}VND</div>
                                            </button>
                                        </div>
                                    </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div className="product-des">
                <div className="tab m-0" >
                    <button className={`fs-4 ${tab == 1 ? "active" : ""}`} onClick={() => handleTab(1)}>
                        <span>Mô Tả</span>
                    </button>
                    <button className={`fs-4 ${tab == 2 ? "active" : ""}`} onClick={() => handleTab(2)}>
                        <span>Đánh Giá</span>
                    </button>
                </div>
                <div className={`protab-des ${tab == 1 ? "active" : "d-none"}`}>
                    <h2 className="pt-5">Mô tả</h2>
                    <p>{product?.description}</p>
                </div>
                <div className={`protab-reviews ${tab == 2 ? "active" : "d-none"}`}>
                    <h2 className="pt-5">Đánh giá</h2>
                    <p className="mb-3">Chưa có đánh giá nào.</p>

                    <div className="row">
                        <form>
                            <p className="comment-notes"><span id="email-notes">Email của bạn sẽ không được hiển thị công khai.</span> <span className="required-field-message">Các trường bắt buộc được đánh dấu <span className="required">*</span></span></p>
                            <div className="comment-rating">
                                <label htmlFor="rating">Đánh giá của bạn</label>
                                <p className="stars">
                                    <span>
                                        <a className="star star-1" href="#">
                                            <FontAwesomeIcon icon={faStar} />
                                        </a>
                                        <a className="star star-2" href="#">
                                            <FontAwesomeIcon icon={faStar} />
                                        </a>
                                        <a className="star star-3" href="#">
                                            <FontAwesomeIcon icon={faStar} />
                                        </a>
                                        <a className="star star-4" href="#">
                                            <FontAwesomeIcon icon={faStar} />
                                        </a>
                                        <a className="star star-5" href="#">
                                            <FontAwesomeIcon icon={faStar} />
                                        </a>
                                    </span>
                                </p>
                                <div className="form-comment mb-3">
                                    <label htmlFor="comment">Nhận xét của bạn&nbsp;
                                        <span className="required">*</span>
                                    </label>
                                    <textarea name="" id="" className="w-100" style={{ height: "200px", borderColor: "#ccc" }}></textarea>
                                </div>
                                <div className="form-auth mb-3">
                                    <label htmlFor="author">Tên&nbsp;
                                        <span className="required">*</span>
                                    </label>
                                    <input type="text" />
                                </div>
                                <div className="form-email mb-3">
                                    <label htmlFor="email">Email&nbsp;
                                        <span className="required">*</span>
                                    </label>
                                    <input type="text" />
                                </div>
                            </div>
                            <button className="p-2 mb-3">
                                <span><strong>Gửi đi</strong></span>
                            </button>
                        </form>
                    </div>

                </div>
            </div>
            <div className="mb-5">
                <div className="row">
                    <div className="text-center clearfix">
                        <h2>Sản phẩm tương tự</h2>

                    </div>
                    {recommend?.map(product => (
                        <div className="col-md-auto product-item" key={product.id}>
                            <div className="product-img">
                                <Link to="#">
                                    <img
                                        src={`${APIENDPOINT}/product/images/${product.imageUrl}`}
                                        alt={product.productName}

                                    />
                                </Link>
                            </div> 
                            <div className='product-info'>
                                <div className="product-title">
                                    <Link to="">{product.productName}
                                    </Link>
                                </div>
                                <div className="product-price">
                                    <span>{product.price} VNĐ</span>
                                </div>
                                <div className='text-center product-action'>
                                    <Link to="#">
                                        <button>CHI TIẾT</button>
                                    </Link>
                                </div>
                            </div>


                        </div>
                    ))}
                </div>


            </div>
        </div>
    )
}

export default Product1
