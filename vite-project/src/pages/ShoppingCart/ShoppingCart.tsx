import { useEffect, useState } from "react";
import NavDrawer from "../../components/AppBar/NavDrawer/NavDrawer";
import Header from "../../components/AppBar/Header/Header";
import Footer from "../../components/AppBar/Footer/Footer";
import BackToTop from "../../components/AppBar/BackTop/BackToTop";
import Contact from "../../components/AppBar/ContactIcon/ContactButton";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDown, faMinus, faPlus } from "@fortawesome/free-solid-svg-icons";
import { ShoppingCartType } from "../../utils/IVegetable";
import { APIENDPOINT } from "../../utils/constant";
import { useAuth } from "../../components/Context/useAuth";
import axios from "axios";
import { Link } from "react-router-dom";
import './ShopppingCart.scss'

const ShoppingCart = () => {
    const [isDrawerOpen, setIsDrawerOpen] = useState(false);
    const [cartItems, setCartItems] = useState<ShoppingCartType[]>([]);
    const [cartItemsChecked, setCartItemsChecked] = useState<ShoppingCartType[]>([]);
    const { user } = useAuth();
    const [isAllChecked, setIsAllChecked] = useState(false);
    const [checkedItems, setCheckedItems] = useState<boolean[]>([]);
    useEffect(() => {
    setCheckedItems(cartItems.map(() => false));
}, [cartItems]);
    console.log(checkedItems);
    const handleCheckAll = () => {
        setIsAllChecked(!isAllChecked);
        setCheckedItems(checkedItems.map(() => !isAllChecked));
        setCartItemsChecked(cartItems);
    };
    const handleCheckItem = (index:number) => {
        setCheckedItems((prevCheckedItems) => {
            const updatedCheckedItems = [...prevCheckedItems];
            updatedCheckedItems[index] = !updatedCheckedItems[index];
            
            return updatedCheckedItems;
        });
        setCartItemsChecked((pre)=>{
            const cartCheckedItems = [...pre];
            cartCheckedItems.push(cartItems[index])
            return cartCheckedItems;
        })
    };
    const toggleDrawer = () => {
        setIsDrawerOpen(!isDrawerOpen);
    };
    const userId = user?.id; // replace with your dynamic userId if needed

    useEffect(() => {
        const fetchCartItems = async () => {
            try {
                const response = await axios.get(`${APIENDPOINT}/ShoppingCart/api/ShoppingCart/userId=${userId}`);
                setCartItems(response.data);
            } catch (error) {
                console.error("Error fetching cart items:", error);
            }
        };
       
        fetchCartItems();
        
    }, [userId]);

    return (
        <>
            <NavDrawer isOpen={isDrawerOpen} onClose={() => setIsDrawerOpen(false)} />
            <Header onMenuClick={toggleDrawer} />
            <div className="page-content">
                <section className="h-100 h-custom" style={{ backgroundColor: "#eee" }}>
                    <div className="container py-5 h-100">
                        <div className="row d-flex justify-content-center align-items-center h-100">
                            <div className="col">
                                <div className="card">
                                    <div className="card-body p-4">

                                        <div className="row">

                                            <div className="col-lg-7">
                                                <h5 className="mb-3"><Link to="#" className="text-body"><i
                                                    className="fas fa-long-arrow-alt-left me-2"></i>Tiếp tục mua sắm</Link></h5>
                                                <hr />

                                                <div className="d-flex justify-content-between align-items-center mb-4">

                                                    <div className="d-flex">
                                                        <div className="SQGY8I pe-2">
                                                            <label className="stardust-checkbox stardust-checkbox--checked">
                                                                <input className="stardust-checkbox__input" type="checkbox"
                                                                    aria-checked="true" aria-disabled="false" tabIndex={0} role="checkbox"
                                                                    aria-label="Click here to select all products" data-sharkid="__1" 
                                                                    onChange={handleCheckAll}
                                                                    />
                                                                </label></div>
                                                        <div>
                                                            <p className="mb-1">Giỏ Hàng</p>
                                                            <p className="mb-0">Bạn có {cartItems.length} đơn trong giỏ hàng</p>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <p className="mb-0"><span className="text-muted">Sắp xếp theo:</span> <Link to="#"
                                                            className="text-body">Giá <FontAwesomeIcon icon={faAngleDown} /></Link></p>
                                                    </div>
                                                </div>
                                                {cartItems.map((item,index) => {
                                                    return (
                                                        <div className="d-flex" key={item.shoppingCart.id}>
                                                        <div className="SQGY8I pe-2 mb-3 align-items-center d-flex">
                                                        <label className="stardust-checkbox stardust-checkbox--checked">
                                                            <input className="stardust-checkbox__input" type="checkbox"
                                                                 aria-disabled="false" tabIndex={0} role="checkbox"
                                                                aria-label="Click here to select all products" data-sharkid="__1"  
                                                                aria-checked={checkedItems[index]} checked={checkedItems[index]}
                                                                onChange={() => handleCheckItem(index)}
                                                                />
                                                            </label></div>
                                                    <div className="card mb-3">
                                                        
                                                        <div className="card-body">
                    
                                                            <div className="d-flex justify-content-between">
                                                                
                                                                <div className="d-flex flex-row align-items-center">
                                                                    <div>
                                                                        <img
                                                                            src={APIENDPOINT + "/product/images/" + item.product.imageUrl}
                                                                            className="img-fluid rounded-3" alt="Shopping item" style={{ width: "65px" }} />
                                                                    </div>
                                                                    <div className="ms-3" style={{ color: "#86bc42" }}>
                                                                        <h5>{item.product.productName}</h5>

                                                                    </div>
                                                                </div>
                                                                <div className="d-flex flex-row align-items-center">
                                                                    <div className='d-flex'>
                                                                        <button data-mdb-button-init="" data-mdb-ripple-init="" className="btn btn-link px-2" data-mdb-button-initialized="true">
                                                                            <FontAwesomeIcon icon={faMinus} />
                                                                        </button>

                                                                        <input id="form1" style={{ width: "50px" }} name="quantity" value={item.shoppingCart.quantity} type="number" className="form-control form-control-sm" />

                                                                        <button data-mdb-button-init="" data-mdb-ripple-init="" className="btn btn-link px-2" data-mdb-button-initialized="true">
                                                                            <FontAwesomeIcon icon={faPlus} />
                                                                        </button>
                                                                    </div>
                                                                    <div style={{ width: '80px' }}>
                                                                        <h5 className="mb-0 d-inline-block" style={{ fontSize: "16px", color: "#86bc42" }}>{item.product.productVariantDTOs[0].unitPrice}VND</h5>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div></div>)
                                                })}




                                            </div>
                                            <div className="col-lg-5">

                                                <div className="card text-white rounded-3">
                                                    <div className="card-body">
                                                        <div className="d-flex justify-content-between align-items-center mb-4">
                                                            <h5 className="mb-0">Thanh Toán</h5>

                                                        </div>




                                                        <hr className="my-4" />

                                                        <div className="d-flex justify-content-between">
                                                            <p className="mb-2">Tiền Hàng</p>
                                                            <p className="mb-2">4798.00 VNĐ</p>
                                                        </div>

                                                        <div className="d-flex justify-content-between">
                                                            <p className="mb-2">Phí giao hàng</p>
                                                            <p className="mb-2">2000 VNĐ</p>
                                                        </div>

                                                        <div className="d-flex justify-content-between mb-4">
                                                            <p className="mb-2">Thành Tiền</p>
                                                            <p className="mb-2">$4818.00</p>
                                                        </div>

                                                        <button type="button" data-mdb-button-init data-mdb-ripple-init className="btn btn-block btn-lg">
                                                            <div className="d-flex justify-content-between">

                                                                <Link to={"/thanh-toan"} state={cartItemsChecked}>Mua Hàng <i className="fas fa-long-arrow-alt-right ms-2"></i></Link>
                                                            </div>
                                                        </button>

                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

            </div>
            <Footer />
            <BackToTop />
            <Contact />
        </>
    )
}

export default ShoppingCart
