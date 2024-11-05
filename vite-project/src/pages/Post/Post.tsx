
import { useEffect, useState } from 'react';
import Header from '../../components/AppBar/Header/Header'
import NavDrawer from '../../components/AppBar/NavDrawer/NavDrawer'
import './Post.scss'
import { post } from '../../utils/IVegetable';
import axios from 'axios';
import { APIENDPOINT } from '../../utils/constant';
import Footer from '../../components/AppBar/Footer/Footer';
import BackToTop from '../../components/AppBar/BackTop/BackToTop';
import Contact from '../../components/AppBar/ContactIcon/ContactButton';
import { Link } from 'react-router-dom';

const Post = () => {
    const [isDrawerOpen, setIsDrawerOpen] = useState(false);
    const [posts, setPosts] = useState<post[]>([]);
    useEffect(() => {
        const fetchPosts = async () => {
            try {
                const response = await axios.get(`${APIENDPOINT}/post/api/post/getall`);
                setPosts(response.data);
                
                // Log dữ liệu mới nhất từ response
            } catch (error) {
                console.error('Lỗi khi lấy dữ liệu:', error);
            }
        };

        fetchPosts();
    }, []);
   
    const toggleDrawer = () => {
        setIsDrawerOpen(!isDrawerOpen);
    };
    return (
        <>
            <NavDrawer isOpen={isDrawerOpen} onClose={() => setIsDrawerOpen(false)} />
            <Header onMenuClick={toggleDrawer} />
            <div className="page-content">
                <div className="container">
                    <div className="row mt-n5">
                        {posts.map((post, index) => {
                            return (<div key={index} className="col-md-6 col-lg-4 mt-5 wow fadeInUp" data-wow-delay=".2s" >
                                <div className="blog-grid">
                                    <div className="blog-grid-img position-relative"><img alt="img" src={post.images[0].url} /></div>
                                    <div className="blog-grid-text p-4">
                                        <h3 className="h5 mb-3"><Link to={`/danh-muc-bai-viet/${post.id}`}>{post.title}</Link></h3>
                                        
                                        <div className="meta meta-style2">
                                            <ul>
                                                <li><Link to={`/danh-muc-bai-viet/${post.id}`}><i className="fas fa-calendar-alt"></i> 2022</Link></li>
                                                <li><Link to={`/danh-muc-bai-viet`}><i className="fas fa-user"></i>{post.author.firstName +" "+ post.author.lastName}</Link></li>
                                                <li><Link to={`/danh-muc-bai-viet/${post.id}`}><i className="fas fa-comments"></i>{post.comments.length}</Link></li>
                                            </ul>
                                        </div>

                                    </div>
                                </div>
                            </div>)
                        })}


                    </div>
                    <div className="row mt-6 wow fadeInUp" data-wow-delay=".6s" >
                        <div className="col-12">
                            <div className="pagination text-small text-uppercase text-extra-dark-gray">
                                <ul>
                                    <li><a href="#!"><i className="fas fa-long-arrow-alt-left me-1 d-none d-sm-inline-block"></i> Prev</a></li>
                                    <li className="active"><a href="#!">1</a></li>
                                    <li><a href="#!">2</a></li>
                                    <li><a href="#!">3</a></li>
                                    <li><a href="#!">Next <i className="fas fa-long-arrow-alt-right ms-1 d-none d-sm-inline-block"></i></a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Footer />
            <BackToTop />
            <Contact />
        </>
    )
}

export default Post
