import LottieVerify from "@/components/lottie/verify";

const SendFilesChecking = () => {
  return ( 
    <div className="flex flex-col h-full justify-center items-center">
      <LottieVerify autoplay loop />
      <h3 className="text-lg font-bold">Verificando arquivos...</h3>
    </div>
  );
}
 
export default SendFilesChecking;