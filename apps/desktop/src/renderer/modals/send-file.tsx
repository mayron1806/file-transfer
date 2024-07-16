import InputFile from "@/components/input-file";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

export function SendFile() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">Enviar arquivos</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Enviar arquivos</DialogTitle>
          <DialogDescription>
            Selecione arquivos do seu computador e compartilhe com qualquer pessoa através do link ou email.
          </DialogDescription>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          <InputFile />
        </div>
        <DialogFooter>
          <Button type="submit">Enviar</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
