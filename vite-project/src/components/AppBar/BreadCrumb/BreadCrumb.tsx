// import { Link, useLocation } from 'react-router-dom';
import './BreadCrumb.scss'

const BreadCrumb = () => {
    // const location = useLocation();
    // const title = location.state?.title || location.pathname.split("/").filter(x => x).pop() || "Default Title";
    
    // // Lấy và tách đường dẫn thành các phần
    // const pathnames = location.pathname.split("/").filter(x => x);
    // console.log(pathnames);
    
    return (
        <div className="breadcrumb">
            <div className="breadcrumb-overlay"></div>
            <div className="breadcrum-content text-center">
                {/* <h2>{title}</h2>
                <span>
                    <span>
                        <Link to="/">Trang chủ</Link>
                    </span>
                    {' >> '}
                    {pathnames.map((value, index) => {
                        const to = `/${pathnames.slice(0, index + 1).join("/")}`;
                        const isLast = index === pathnames.length - 1;

                        return isLast ? (
                            <span key={to}> {value} </span>
                        ) : (
                            <span key={to}>
                                <Link to={to}>{value}</Link> {' >> '}
                            </span>
                        );
                    })}
                </span> */}
            </div>
        </div>
    );
}

export default BreadCrumb;
