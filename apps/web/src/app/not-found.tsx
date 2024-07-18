import LottieNotFound from "@/components/lottie/not-found";
import Title from "@/components/title";
import Link from "next/link";

const NotFound = () => {
  return (
    <div className="h-screen container flex flex-col gap-5 items-center justify-center ">
      <div className="w-9/12 min-w-64">
        <LottieNotFound autoplay loop />
      </div>
      <Title>Página não encontrada</Title>
      <Link className="underline" href="/">Voltar para home</Link>
    </div>
  );
}
 
export default NotFound;