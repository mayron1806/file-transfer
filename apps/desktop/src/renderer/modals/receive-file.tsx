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

export function ReceiveFile() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">Receber arquivos</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Receber arquivos</DialogTitle>
          <DialogDescription>
            Crie um link para receber arquivos de outra pessoa com o link
          </DialogDescription>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          <InputFile />
        </div>
        <DialogFooter>
          <Button type="submit">Criar link</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
