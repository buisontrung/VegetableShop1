
import { useEffect, useState } from 'react'
import './HomeProduct.scss'
import {Category, Product} from '../../../utils/IVegetable';
import axios from 'axios';
import { APIENDPOINT } from '../../../utils/constant';
import { Link } from 'react-router-dom';

const HomeProduct = () => {
  const [tab, setTab] = useState<Category[]>([]);
  const [activeTab, setActiveTab] = useState<number>(1);
  const [productTab,setProductTab]= useState<Record<number, Product[]>>({});
  const openTab = (categoryId: number) => {
    setActiveTab(categoryId); // Cập nhật tab active
  };
  useEffect(() => {
    axios.get(`${APIENDPOINT}/product/api/productcategory/getall`)
      .then(res => {
        setTab(res.data);
      })
      .catch(err => {
        console.log(err);
      })
  }, [])
  useEffect(() => {
    axios.get(`${APIENDPOINT}/product/api/product/getall`)
      .then(res => {
        const grouped = res.data.reduce((arr: Record<number, Product[]>, product: Product) => {
          if (!arr[product.productCategoryId]) {
            arr[product.productCategoryId] = [];
          }
          arr[product.productCategoryId].push(product);
          return arr;
        }, {});
        setProductTab(grouped);
        
      })
      .catch(err => {
        console.log(err);
      })
  }, [])

  return (
    <div className='home__products'>
      <div className="pt-5 pb-5">
        <div className="container">
          <div className="row text-center">
            <div className="section-title">
              <h2 className="display-4 fw-normal">Sản Phẩm Của Chúng Tôi</h2>
            </div>
            <div className="section-content">
              <div className="tab">
                {tab.length > 0 ? (
                  tab.map((item) => (
                    <button data-id={item.id} onClick={() => openTab(item.id)}
                      key={item.id}
                      className={`openTab px-4 pt-1 pb-1 mx-2 ${activeTab == item.id ?"active":""}`}
                    >
                      {item.productCategoryName}
                    </button>
                  ))
                ) : (
                  <p>Không có sản phẩm nào</p>
                )}
              </div>
              {Object.keys(productTab).map(categoryId => {
                const numCategoryId = Number(categoryId);
                return(
                  <div id={`tab-${categoryId}`} key={categoryId} style={{ display: activeTab === Number(categoryId) ? 'block' : 'none' }}>
                    <div className="container">
                    <div className="row ">
                      {productTab[numCategoryId].map(product => (
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
                  </div>)
              })}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default HomeProduct
