import './Main.css'

//import video1 from './public/images/video1.mp4';
//import video1 from 'D:/reactprojects/fastx/public/images/video1.mp4';


function main(){
    return(
<div class="banner">
            <video autoplay loop muted plays-inline>
                {/* <source src={video1} type="video/mp4"/> */}
            </video>
          
            <div className="content">
                <h1>FastX:Explore the road adventure</h1>
                <div>
                    <a href="home.html">
                    <button type="button">Explore</button>
                </a>
                </div>
            </div>
        </div>
    )
}
export default main;
