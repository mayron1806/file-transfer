'use client';
import { CSSProperties } from "react";
import LottieError from "./lottie/error";
import LottieNotFound from "./lottie/not-found";
import LottieServerError from "./lottie/server-error";
import LottieUnauthorized from "./lottie/unauthorized-error";
import { Button } from "./ui/button";

type Props = {
  error: {
    message: string;
    status?: number;
  };
  goBackAction?: () => Promise<unknown>;
  goBackActionLabel?: string;
}
const Error = ({ error, goBackAction, goBackActionLabel }: Props) => {
  const iconStyle: CSSProperties = {
    width: '100%',
    maxWidth: '100vw'
  }
  let icon;
  switch (error.status) {
    case 401:
    case 403:
      icon = <LottieUnauthorized autoplay style={iconStyle} keepLastFrame />;
      break;
    case 404:
      icon = <LottieNotFound autoplay style={iconStyle} keepLastFrame />;
      break;
    case 500:
    case 503:
      icon = <LottieServerError autoplay style={iconStyle} keepLastFrame />;
      break;
    default:
      icon = <LottieError autoplay style={iconStyle} keepLastFrame />;
  }
  const action = () => goBackAction?.();
  return (
    <div className="h-screen flex flex-col items-center justify-center gap-4 text-center">
      {icon}
      <p className="text-lg">{error.message}</p>
      {
        goBackAction && (
          <form action={action}>
            <Button>{goBackActionLabel ? goBackActionLabel : 'Voltar' }</Button>
          </form>
        )
      }
    </div>
  );
}
 
export default Error;