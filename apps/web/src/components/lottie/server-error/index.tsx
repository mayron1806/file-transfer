'use client';
import { Player } from '@lottiefiles/react-lottie-player';
import animation from './animation.json'
import { LottieElementsProps } from '../types';
const LottieServerError = (props: LottieElementsProps) => {
  return ( 
    <Player src={animation} {...props} />
  );
}
 
export default LottieServerError;