'use client';
import LottieCheck from "@/components/lottie/check";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { CopyIcon } from "lucide-react";
type Props = {
  link: string;
}
const SendFilesFinished = ({ link }: Props) => {

  const addToClipboard = () => {
    navigator.clipboard.writeText(link);
  }
  return (
    <div className="flex flex-col h-full justify-center items-center">
      <LottieCheck autoplay keepLastFrame />
      <h3 className="text-xl font-bold">Arquivos enviados!</h3>
      <div className="mt-2 flex w-full justify-center gap-2 text-center">
        <Input className="w-1/2" value={link} readOnly />
        <Button size="icon" onClick={addToClipboard}>
          <CopyIcon className="h-4 w-4"/>
        </Button>
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