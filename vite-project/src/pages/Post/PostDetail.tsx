import { useEffect, useState } from "react";
import { useParams } from "react-router-dom"
import { comment, post, user } from "../../utils/IVegetable";
import axios from "axios";
import { APIENDPOINT } from "../../utils/constant";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "../../components/Context/useAuth";
import NavDrawer from "../../components/AppBar/NavDrawer/NavDrawer";
import Header from "../../components/AppBar/Header/Header";
import "./PostDetail.scss"
import { useCookies } from "react-cookie";
import CategoryTable from "../../components/AppBar/CategoryTable/CategoryTable";

const PostDetail = () => {
  const { id } = useParams();

  const [post, setPost] = useState<post | null>(null);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [comments, setComments] = useState<comment[]>([]);
  const [newComment, setNewComment] = useState<string>("");
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [cookies] = useCookies(['accessToken', 'refreshToken']);
  const { user } = useAuth();
  const fetchPostById = async (id: string) => {
    try {
      const response = await axios(`${APIENDPOINT}/post/api/post/${id}`);

      setPost(response.data);
      setComments(response.data.comments || []);
    } catch (error) {
      console.error('Error fetching post:', error);
    }
  };

  // Gọi hàm fetch khi component mount hoặc id thay đổi
  useEffect(() => {
    if (id) {
      fetchPostById(id);
    }
  }, [id]);



  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7237/commentHub")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection.start()
      .then(() => {
        console.log('Connected to SignalR hub');
        if (id) {

          // Gọi phương thức JoinGroup để tham gia vào nhóm với postId cụ thể
          newConnection.invoke("JoinGroup", id)
            .catch(err => console.log('Error joining group: ' + err));

          newConnection.on("ReceiveComment", (receivedPostId: string, userJson: string, message: string) => {
            if (receivedPostId === id) {
              const newUser: user = JSON.parse(userJson); // Phân tích cú pháp chuỗi JSON thành đối tượng

              const newComment: comment = {
                author: newUser,
                contents: message,
                _id: "some_generated_id", // Có thể là ID bình luận được sinh ra từ server
                createdAt: new Date().toISOString(), // Thời gian hiện tại
                updateAt: new Date().toISOString(), // Thời gian hiện tại
              };
              setComments(prev => [...prev, newComment]);
            }
          });

          newConnection.on("GroupJoined", (joinedPostId: string) => {
            console.log(`Joined group: ${joinedPostId}`);
          });
        }

      })
      .catch(() => console.error('Connection failed: ' + id));

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, [id]);

  const handleSubmitComment = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (newComment.trim() === "") return;

    try {
      const res = await axios.post(`${APIENDPOINT}/post/api/Post/createComment/?id=${id}`, {
        contents: newComment,
        author: {
          id: user?.id,
          firstName: user?.firstName,
          lastName: user?.lastName
        }
      });

      if (res.status === 201 && connection) {
        await connection.invoke("SendComment", post?.id, cookies.accessToken, newComment); // Thay "username" bằng tên người dùng thực tế
        setNewComment(""); // Reset comment input
      }
    } catch (error) {
      console.error('Error submitting comment:', error);
    }
  };
  const toggleDrawer = () => {
    setIsDrawerOpen(!isDrawerOpen);
  };

  return (
    <>

      <NavDrawer isOpen={isDrawerOpen} onClose={() => setIsDrawerOpen(false)} />
      <Header onMenuClick={toggleDrawer} />
      <div className="page-content" style={{backgroundColor:"#f2f3f8"}}>
        <div className="container pb50">
          <div className="row">
            <div className="col-md-9 mb40" style={{backgroundColor:"#fff"}} >
              <article>
                <img src={post?.images[0].url} alt="" className="img-fluid mb30" />
                <div className="post-content">
                  <h3>{post?.title}</h3>
                  <ul className="post-meta list-inline">
                    <li className="list-inline-item">
                      <i className="fa fa-user-circle-o"></i> <a href="#">{post?.author.firstName +" "+ post?.author.lastName}</a>
                    </li>
                    <li className="list-inline-item">
                      <i className="fa fa-calendar-o"></i> <a href="#"> 2024 </a>
                    </li>
                    <li className="list-inline-item">
                      <i className="fa fa-tags"></i> <a href="#">Rau cu</a>
                    </li>
                  </ul>
                  {post?.contents.map(content=>{
                    return(
                      <>
                      <h4 className="mb-4">{content.title}</h4>
                      <p>{content.value}</p>
                      {content.imageUrl?<img className="content-img mb-4" src={content.imageUrl} alt="" />:""}
                      </>
                    )
                  })}
                  <ul className="list-inline share-buttons">
                    <li className="list-inline-item">Share Post:</li>
                    <li className="list-inline-item">
                      <a href="#" className="social-icon-sm si-dark si-colored-facebook si-gray-round">
                        <i className="fa fa-facebook"></i>
                        <i className="fa fa-facebook"></i>
                      </a>
                    </li>
                    <li className="list-inline-item">
                      <a href="#" className="social-icon-sm si-dark si-colored-twitter si-gray-round">
                        <i className="fa fa-twitter"></i>
                        <i className="fa fa-twitter"></i>
                      </a>
                    </li>
                    <li className="list-inline-item">
                      <a href="#" className="social-icon-sm si-dark si-colored-linkedin si-gray-round">
                        <i className="fa fa-linkedin"></i>
                        <i className="fa fa-linkedin"></i>
                      </a>
                    </li>
                  </ul>

                  <hr className="mb40" />
                  <h4 className="mb40 text-uppercase font500">Danh sách bình luận</h4>
                  {comments.map(comment => {
                    return (<><div className="mb-4 d-flex align-items-center justify-content-between">
                      <div className="d-flex  align-items-center"><i className="mr-3 fa fa-user-circle-o fa-3x pe-3"></i>
                      <div><h5 className="mb-0">{comment.author.firstName + " " + comment.author.lastName}</h5></div></div>
                      
                      {/* <div className="media-body">
                        <h5 className="mt-0 font400 clearfix mb-0">
                          <a href="#" className="float-right">Reply</a>
                          </h5>
                        
                      </div> */}
                    </div>
                    <p >{comment.contents}</p></>)
                  })}

                  <hr className="mb40" />
                  <h4 className="mb40 text-uppercase font500">Phản hồi</h4>
                  <form role="form" onSubmit={handleSubmitComment}>

                    <div className="form-group">

                      <textarea className="form-control" rows={5} placeholder="Bình luận" value={newComment} onChange={(e) => setNewComment(e.target.value)}></textarea>
                    </div>
                    <div className="clearfix float-right mt-4">
                      <button type="submit" className="btn btn-primary btn-lg">Gửi bình luận</button>
                    </div>
                  </form>
                </div>
              </article>


            </div>
            <div className="col-md-3 mb40">
              <div className="mb40">
                <form>
                  <div className="input-group">
                    <input type="text" className="form-control" placeholder="Search..." aria-describedby="basic-addon2" />
                    <button className="input-group-addon" id="basic-addon2"><i className="fa fa-search"></i></button>
                  </div>
                </form>
              </div>

              <div className="mb40">
                <CategoryTable />
              </div>


            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default PostDetail
