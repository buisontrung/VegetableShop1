import { useEffect, useState } from 'react'
import { Link, useLocation } from 'react-router-dom'
import { Product } from '../../../utils/IVegetable';
import axios from 'axios';
import { APIENDPOINT } from '../../../utils/constant';
import './Product.scss'
const Products = () => {
    const location = useLocation();
    const categoryName = location.state?.name;
    const id = location.state?.id || '';
    const [products, setProducts] = useState<Product[]>([]);
    const [quantity,setQuantity]= useState(0);
    const [orderbyIndex, setOrderbyIndex] = useState(1);
    const [pageNumber,setPageNumber] =useState(1);

    // Function to handle ordering
    const handleOrderbySubmit = (index: number) => {
        setOrderbyIndex(index);
    }
    useEffect(()=>{
        const fetchQuantity = async () => {
            try {
                const res = await axios.get(`${APIENDPOINT}/product/api/Product/getcountproduct/$${id}`);
                setQuantity(res.data);
            } catch (err) {
                console.log(err);
            }
        };

        fetchQuantity();
    },[id])
    // Function to get the appropriate API endpoint for sorting
    

    useEffect(() => {
        const getSortingEndpoint = () => {
            let endpoint = `${APIENDPOINT}/product/api/Product/productscategoryid=${id}`;
            switch (orderbyIndex) {
                case 1:
                    endpoint += '&getbyorder=default';
                    break;
                case 2:
                    endpoint += '&getbyorder=popularity';
                    break;
                case 3:
                    endpoint += '&getbyorder=rating';
                    break;
                case 4:
                    endpoint += '&getbyorder=date';
                    break;
                case 5:
                    endpoint += '&getbyorder=price';
                    break;
                case 6:
                    endpoint += '&getbyorder=price_desc';
                    break;
                default:
                    endpoint += '';
            }
            return endpoint;
        };

        const fetchProducts = async () => {
            try {
                const endpoint = getSortingEndpoint();
                const res = await axios.get(endpoint+`&pz=3&pn=${pageNumber}`);
                setProducts(res.data);
            } catch (err) {
                console.log(err);
            }
        };
        
        fetchProducts();
    }, [orderbyIndex, id,pageNumber]);
    const handlePageChange = (page: number) => {
        setPageNumber(page);

    };
    const renderPaginationItems = () => {
        const items = [];
        for (let i = 0; i <= quantity/3; i+=1) {
            const a = i+1;
            items.push(
                <li key={a} onClick={() => handlePageChange(a)} className={a === pageNumber ? 'active' : ''}>
                    {a}
                </li>
            );
            
        }
        return items;
    }
    return (
        <>
            <header><h1>{categoryName}</h1></header>
            <p className='result-count'>Hiển thị {products.length}-{quantity} kết quả</p>
            <form>
                <select 
                    name="orderby" 
                    className="orderby" 
                    aria-label="Đơn hàng của cửa hàng" 
                    onChange={(e) => handleOrderbySubmit(Number(e.target.value))} // Handle select change
                >
                    <option value="1">Thứ tự mặc định</option>
                    <option value="2">Thứ tự theo mức độ phổ biến</option>
                    <option value="3">Thứ tự theo điểm đánh giá</option>
                    <option value="4">Mới nhất</option>
                    <option value="5">Thứ tự theo giá: thấp đến cao</option>
                    <option value="6">Thứ tự theo giá: cao xuống thấp</option>
                </select>
            </form>
            <div className='row'>
                {products.map((product) => (
                    <div className="col-md-auto product-item" key={product.id}>
                        <div className="product-img text-center">
                            <a href="#/">
                                <img src={`https://localhost:7000/product/images/${product.imageUrl}`} alt={product.productName} />
                            </a>
                        </div>
                        <div className="product-info">
                            <div className="product-title">
                                <Link to={`/san-pham/${product.id}`}>{product.productName}</Link>
                            </div>
                            <div className="product-price">
                                <span>{product.price} VNĐ</span>
                            </div>
                            <div className="text-center product-action">
                                <a href="#/">
                                    <button>CHI TIẾT</button>
                                </a>
                            </div>
                        </div>
                    </div>
                ))}
                
            </div>
            <nav className='woocommerce-pagination'>

                    <ul>{renderPaginationItems()}</ul>
                </nav>
        </>
    )
}

export default Products;
