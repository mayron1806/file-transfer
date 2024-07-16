import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import FileTransfer from "@/assets/file-transfer.svg";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import Title from "@/components/title";
import Description from "@/components/description";
import { zodResolver } from '@hookform/resolvers/zod';
import { CreateAccountSchema, createAccountSchema } from "@/validation/create-account-schema";
export default function CreateAccount() {
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors }} = useForm<CreateAccountSchema>({
    resolver: zodResolver(createAccountSchema),
  })
  return (
    <div className="w-full lg:grid h-full lg:min-h-[600px] lg:grid-cols-2 xl:min-h-[800px]">
      <div className="flex items-center justify-center py-12">
        <div className="mx-auto grid w-[350px] gap-6">
          <div className="grid gap-2 text-center">
            <Title>Criar conta</Title>
            <Description>Cadastre-se para fazer transferencias</Description>
          </div>
          <form className="grid gap-4">
            <div className="grid gap-2">
              <Label htmlFor="name">Nome</Label>
              <Input {...register('name')} type="text" required />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                {...register('email')}
                type="email"
                placeholder="m@example.com"
                required
              />
            </div>
            <div className="grid gap-2">
              <div className="flex items-center">
                <Label htmlFor="password">Senha</Label>
              </div>
              <Input {...register('password')} type="password" required />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="confirm-password">Confirmar senha</Label>
              <Input {...register('confirm-password')} type="password" required />
            </div>
            <Button type="submit" className="w-full">
              Criar conta
            </Button>
            <Button variant="outline" className="w-full">
              Entrar com Google
            </Button>
          </form>
          <div className="mt-4 text-center text-sm">
            Já possui uma conta?{" "}
            <a onClick={() => navigate('/auth/signin')} className="underline cursor-pointer">
              Entrar
            </a>
          </div>
        </div>
      </div>
      <div className="hidden bg-muted lg:block">
        <div className="flex h-full items-center justify-center">
          <FileTransfer />
        </div>
      </div>
    </div>
  )
}
