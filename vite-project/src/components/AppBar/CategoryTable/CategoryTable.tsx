import { useEffect, useState } from 'react'
import './CategoryTable.scss'
import { Category } from '../../../utils/IVegetable'
import axios from 'axios'
import { APIENDPOINT } from '../../../utils/constant'
import { Link } from 'react-router-dom'

const CategoryTable = () => {
    const [category, setCategory] = useState<Category[]>([])
    useEffect(() => {
        axios.get(`${APIENDPOINT}/product/api/productcategory/getall`)
            .then(res => {
                setCategory(res.data);
            })
            .catch(err => {
                console.log(err);
            })
    }, [])
    return (
        <>
            <div className="product-category_tb">
                <div className="product-category-title text-center">
                    <h3>Danh Mục Sản Phẩm</h3>
                </div>
                <ul>
                    {category.map((categoryItem) => (
                        
                            <li key={categoryItem.id}><Link state={{ id: categoryItem.id,name:categoryItem.productCategoryName}}  to={"/danh-muc-san-pham/" + categoryItem.productCategoryName}>{categoryItem.productCategoryName}</Link></li>
                        
                    ))}
                </ul>
            </div>
            <div className='product_tb'>
            <div className="product-category-title text-center">
                    <h3>Sản Phẩm</h3>
                </div>
                <ul>
                    <li><Link to="">CÀ RỐT</Link></li>
                    <li><Link to="">ĐẬU ĐŨA</Link></li>
                    <li><Link to="">ĐẬU BẮP</Link></li>
                    <li><Link to="">DƯA LEO</Link></li>
                    <li><Link to="">KHỔ QUA</Link></li>
                </ul>
            </div>
        </>
    )
}

export default CategoryTable
