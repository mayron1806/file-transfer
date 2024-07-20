'use client';

import { Button } from "@/components/ui/button";
import { UploadCloud } from "lucide-react";
import { Sheet, SheetContent, SheetHeader, SheetTrigger, SheetDescription, SheetTitle } from "../../ui/sheet";
import { useOrganization } from "@/context/organization-context";
import { SendFilesSchema } from "@/validation/send-files";
import { ScrollArea } from "../../ui/scroll-area";
import { useState } from "react";
import { useToast } from "../../ui/use-toast";
import { useAuth } from "@/hooks/use-auth";
import { Semaphore } from "@/lib/semaphore";
import { prepareSendFilesTransfer } from "@/usecases/create-send-transfer";
import { HttpError } from "@/errors/HttpError";
import SendFilesForm from "./form";
import SendingFiles, { UploadProgress } from "./sending";
import { uploadFile } from "@/usecases/upload-file";
import SendFilesChecking from "./checking";
import SendFilesFinished from "./finish";
import { confirmFileTransfer } from "@/usecases/confirm-file-transfer";

const MAX_UPLOAD_FILES_CONCURRENCY = 5;
const NewSendFilesDialog = () => {
  const { organization } = useOrganization();
  const { accessToken } = useAuth();
  const { toast } = useToast();
  const [submitState, setSubmitState] = useState<"idle" | "preparing" | "sending" | "checking" | "finished">("idle");
  const [uploadProgress, setUploadProgress] = useState<Array<UploadProgress>>([]);
  const [filesState, setFilesState] = useState<{ total: number, errors: number }>({ total: 0, errors: 0 });
  
  const handleUploadFile = async (file: File, url: string) => {
    try {
      await new Promise<void>((resolve, reject) => {
        const { abort, start } = uploadFile(
          url, 
          file, 
          (progress) => {
            setUploadProgress(prev => {
              const index = prev.findIndex(item => item.fileName === file.name);
              if (index >= 0) {
                prev[index].progress = progress;
                return [...prev];
              }
              prev.push({ fileName: file.name, progress, finished: false, abort, size: file.size });
              return [...prev];
            });
          },
          (success) => {
            setUploadProgress(prev => {
              const index = prev.findIndex(item => item.fileName === file.name);
              if (index >= 0) {
                prev[index].finished = true;
                prev[index].error = success ? undefined : "Erro ao fazer upload";
                return [...prev];
              }
              return prev;
            });
            success ? resolve() : reject();
          }
        );
        start().catch(reject);
      });
    } catch (error) {
      if (error instanceof Error) {
        toast({
          title: "Erro ao fazer upload",
          description: error.message,
          variant: "destructive",
        });
        setUploadProgress(prev => {
          const index = prev.findIndex(item => item.fileName === file.name);
          if (index >= 0) {
            prev[index].error = error.message;
            prev[index].finished = false;
            return [...prev];
          }
          prev.push({ fileName: file.name, progress: 0, finished: false, error: error.message, size: file.size });
          return [...prev];
        });
      }
    }
  };
  
  const onSubmitForm = async (body: SendFilesSchema) => {
    setSubmitState("preparing");
    const files = body.files as FileList;
    try {
      const prepareData = await prepareSendFilesTransfer(accessToken, organization.id, {
        files: Object.values(files).map(file => ({ name: file.name, size: file.size, contentType: file.type })),
        emailsDestination: body.emailsDestination,
        expiresAt: body.expiresAt,
        expiresOnDownload: body.expiresOnDownload,
        message: body.message,
        name: body.name,
        password: body.password,
      });
      setSubmitState("sending");

      // enviar arquivos atraves das urls assinadas
      const semaphore = new Semaphore(MAX_UPLOAD_FILES_CONCURRENCY);
      const promises = prepareData.urls.map(async (url, index) => {
        await semaphore.acquire();
        await handleUploadFile(files[index], url);
        semaphore.release();
      });
      // aguardar todas as promises serem resolvidas
      await Promise.all(promises);

      
      // enviar confirmação de envio para o servidor
      setSubmitState("checking");
      const confirmationResult = await confirmFileTransfer(accessToken, organization.id, prepareData.transferId);
      setFilesState({
        total: Object.values(files).length,
        errors: confirmationResult.filesWithError.length
      });

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
          <UploadCloud className="mr-2 h-4 w-4" />
          Enviar
        </Button>
      </SheetTrigger>
      <SheetContent className="w-full sm:max-w-lg">
        { 
          ["idle", "preparing"].includes(submitState) && 
          <ScrollArea className="h-full pr-2.5">
            <SheetHeader>
              <SheetTitle>Enviar arquivos</SheetTitle>
              <SheetDescription>Envie arquivos para qualquer pessoa.</SheetDescription>
            </SheetHeader>
            <SendFilesForm onSubmit={onSubmitForm} />
          </ScrollArea>
        }
        { submitState === "sending" && <SendingFiles uploadProgress={uploadProgress} /> }
        { submitState === "checking" && <SendFilesChecking /> }
        { submitState === "finished" && <SendFilesFinished errorsCount={filesState.errors} totalFiles={filesState.total} /> }
      </SheetContent>
    </Sheet>
  );
}
export default NewSendFilesDialog;