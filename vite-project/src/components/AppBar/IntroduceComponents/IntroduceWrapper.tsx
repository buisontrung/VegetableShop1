
import { introducearr } from '../../../utils/array'

const IntroduceWrapper = () => {
    return (
        <>
            <header><h1>Giới Thiệu</h1></header>
            <div className='hd-content'>
                {introducearr.content.map((paragraph, index) => (
                    <p key={index}>{paragraph}</p>
                ))}
            </div>
        </>
    )
}

export default IntroduceWrapper
