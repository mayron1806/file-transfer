import LottieCheck from "@/components/lottie/check";
import { Button } from "@/components/ui/button";
type Props = {
  totalFiles: number;
  errorsCount: number;
}
const SendFilesFinished = ({ errorsCount, totalFiles }: Props) => {
  return (
    <div className="flex flex-col h-full justify-center items-center">
      <LottieCheck autoplay keepLastFrame />
      <h3 className="text-xl font-bold">Arquivos enviados!</h3>
      <div className="mt-2 flex w-full justify-evenly text-center">
        <Item label="Arquivos enviados" value={totalFiles - errorsCount} />
        <Item label="Arquivos com erro" value={errorsCount} />
      </div>
      <Button className="mt-6">Fechar</Button>
    </div>
  );
}
type ItemProps = {
  value: number;
  label: string;
}
const Item = ({ label, value }: ItemProps) => {
  return (
    <div className="flex flex-col gap-2">
      <h4 className="text-3xl font-bold">{value}</h4>
      <p className=" text-sm font-medium">{label}</p>
    </div>
  );
}
export default SendFilesFinished;