import { useEffect, useState } from "react";
import './BackToTop.scss'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDoubleUp } from "@fortawesome/free-solid-svg-icons";
const BackToTop = () => {
    const [isScrolled, setIsScrolled] = useState(false);
    const handleScroll = () => {
        if (window.scrollY > 150) {
            setIsScrolled(true);
        } else {
            setIsScrolled(false);
        }
    };
    useEffect(() => {
        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
      }, []);
    return (
        <div id='back-to-top' className={`${!isScrolled?"d-none":""}`}>
            <div className="back-circle">
                <div className="circle"></div>
                <div className="circle"></div>
                <div className="circle"></div>
                
                <FontAwesomeIcon icon={faAngleDoubleUp}/>
                

            </div>
        </div>
    )
}

export default BackToTop
