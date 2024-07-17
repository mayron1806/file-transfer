'use client';
import { Player } from '@lottiefiles/react-lottie-player';
import animation from './animation.json';
const LottieSuccess = () => {
  return ( 
    <Player src={animation} style={{ width: "400px", height: "400px" }} autoplay keepLastFrame />
  );
}
 
export default LottieSuccess;