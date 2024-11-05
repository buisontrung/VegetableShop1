import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import './NewDrawer.scss'
import { faWindowClose } from '@fortawesome/free-solid-svg-icons'
import { Link } from 'react-router-dom'


interface HeaderProps {
  isOpen:boolean;
  onClose: () => void; // Prop được truyền vào là một hàm
}

const NavDrawer:  React.FC<HeaderProps> = ({ isOpen,onClose }) => {
    
  return (
    <div className={`drawer drawer--right ${isOpen ? 'open' : ''}`}>
      <div className="drawer__header">
        <div className="container text-end drawer__close">
            <button  onClick={onClose}><span>Đóng</span>
            <FontAwesomeIcon icon={faWindowClose} className="m-auto px-1"/></button>
        </div>

      </div>
      <div className='container'>
      <ul className='row'>
        <li><Link to="#" className='mobile-nav__link'>TRANG CHỦ</Link></li>
        <li><Link to="#" className='mobile-nav__link'>GIỚI THIỆU</Link></li>
        <li><Link to="#" className='mobile-nav__link'>SẢN PHẨM</Link></li>
        <li><Link to="#" className='mobile-nav__link'>LIÊN HỆ</Link></li>
        <li><Link to="#" className='mobile-nav__link'>ĐẶT HÀNG</Link></li>
      </ul>
      </div>
    </div>
  )
}

export default NavDrawer
