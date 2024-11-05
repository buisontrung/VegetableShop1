
import './BlogWrapper.scss'

const BlogWrapper = ({ children }: { children: React.ReactNode }) => {
    // Kiểm tra nếu `children` là mảng (tức là có nhiều phần tử con)
    const childrenArray = Array.isArray(children) ? children : [children]

    return (
        <div className='blog-wrapper'>
            <div className="container">
                <div className="row justify-content-between">
                    <div className='col-12 col-md-3'>
                    {childrenArray[0]}
                    </div>
                    <div className='col-12 col-md-7'>
                    {childrenArray[1]}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default BlogWrapper
