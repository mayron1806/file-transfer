'use client';

import { Button } from "@/components/ui/button";
import { DownloadCloud } from "lucide-react";
import { Sheet, SheetContent, SheetHeader, SheetTrigger, SheetDescription, SheetTitle } from "../../ui/sheet";
import { useOrganization } from "@/context/organization-context";
import { ScrollArea } from "../../ui/scroll-area";
import { useState } from "react";
import { useToast } from "../../ui/use-toast";
import { useAuth } from "@/hooks/use-auth";
import { prepareReceiveFilesTransfer } from "@/usecases/prepare-receive-files-transfer";
import { HttpError } from "@/errors/HttpError";
import ReceiveFilesForm from "./form";
import SendFilesFinished from "./finish";
import { ReceiveFilesSchema } from "@/validation/receive-files";

const NewReceiveFilesDialog = () => {
  const { organization } = useOrganization();
  const { accessToken } = useAuth();
  const { toast } = useToast();
  const [submitState, setSubmitState] = useState<"idle" | "loading" | "finished">("idle");
  const [link, setLink] = useState<string>("");
  
  const onSubmitForm = async (body: ReceiveFilesSchema) => {
    setSubmitState("loading");
    try {
      console.log(body);
      const prepareData = await prepareReceiveFilesTransfer(accessToken, organization.id, {
        expiresAt: body.expiresAt,
        message: body.message !== '' ? body.message : undefined,
        name: body.name !== '' ? body.name : undefined,
        password: body.password !== '' ? body.password : undefined,
        acceptedFiles: body.acceptedFiles,
        maxFiles: body.maxFiles,
        maxSize: body.maxSize,
      });
      setLink(prepareData.uploadUrl);
      setSubmitState("finished");
    } catch (error) {
      if(error instanceof HttpError) {
        console.log(error.message);
        
        toast({
          title: "Erro ao enviar arquivos",
          description: error.message,
          variant: "destructive",
        });
      }
      if(error instanceof Error) {
        toast({
          title: "Erro ao enviar arquivos",
          description: error.message,
          variant: "destructive",
        });
      }
    }
  };
  
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="default">
          <DownloadCloud className="mr-2 h-4 w-4" />
          Receber arquivos
        </Button>
      </SheetTrigger>
      <SheetContent className="w-full sm:max-w-lg">
        { 
          ["idle", "loading"].includes(submitState) && 
          <ScrollArea className="h-full pr-2.5">
            <SheetHeader>
              <SheetTitle>Receber arquivos</SheetTitle>
              <SheetDescription>Envie arquivos para qualquer pessoa.</SheetDescription>
            </SheetHeader>
            <ReceiveFilesForm onSubmit={onSubmitForm} />
          </ScrollArea>
        }
        { submitState === "finished" && <SendFilesFinished link={link} /> }
      </SheetContent>
    </Sheet>
  );
}
export default NewReceiveFilesDialog;