'use client';
import { Player } from '@lottiefiles/react-lottie-player';
import animation from './animation.json';
import { LottieElementsProps } from '../types';

const LottieNotFound = (props: LottieElementsProps) => {
  return ( 
    <Player src={animation} {...props} />
  );
}

export default LottieNotFound;