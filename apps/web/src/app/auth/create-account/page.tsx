'use client';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import FileTransfer from "@/assets/file-transfer.svg";
import { useForm } from "react-hook-form";
import Title from "@/components/title";
import Description from "@/components/description";
import { zodResolver } from '@hookform/resolvers/zod';
import { CreateAccountSchema, createAccountSchema } from "@/validation/create-account";
import Link from "next/link";
import { useToast } from "@/components/ui/use-toast";
import ErrorMessage from "@/components/error-message";
import { createAccount } from "@/usecases/create-account";

const CreateAccount = () => {
  const { toast } = useToast();
  const { register, handleSubmit, formState: { isSubmitting, errors }, setError } = useForm<CreateAccountSchema>({
    resolver: zodResolver(createAccountSchema)
  });
  const isValid = Object.values(errors).filter(e => e.message).length === 0;
  console.log(errors);
  
  const submit = handleSubmit(async (data) => {
    try {
      await createAccount(data);
      toast({
        title: 'Conta criada',
        description: 'Conta criada com sucesso, confirme seu email para ativar sua conta',
        duration: 1000 * 60 // 1 min
      });
    } catch (error) {
      if (error instanceof Error) {
        toast({
          title: 'Erro ao criar conta',
          description: error.message,
          variant: 'destructive',
        });
      }
    }
  });
  return ( 
    <div className="w-full lg:grid h-screen lg:min-h-[600px] lg:grid-cols-2 xl:min-h-[800px]">
      <div className="flex items-center justify-center py-12">
        <div className="mx-auto grid w-[350px] gap-6">
          <div className="grid gap-2 text-center">
            <Title>Criar conta</Title>
            <Description>Cadastre-se para fazer transferencias</Description>
          </div>
          <form onSubmit={submit} method="POST" className="grid gap-4">
            <div className="grid gap-2">
              <Label htmlFor="name">Nome</Label>
              <Input {...register('name')} type="text" required />
              <ErrorMessage message={errors.name?.message} />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                {...register('email')}
                type="email"
                placeholder="m@example.com"
                required
              />
              <ErrorMessage message={errors.email?.message} />
            </div>
            <div className="grid gap-2">
              <div className="flex items-center">
                <Label htmlFor="password">Senha</Label>
              </div>
              <Input {...register('password')} type="password" required />
              <ErrorMessage message={errors.password?.message} />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="confirm-password">Confirmar senha</Label>
              <Input {...register('confirm-password')} type="password" required />
              <ErrorMessage message={errors["confirm-password"]?.message} />
            </div>
            <ErrorMessage message={errors.root?.message} />
            <Button 
              type="submit" 
              className="w-full"
              disabled={isSubmitting || !isValid}
              loading={isSubmitting}
            >
              Criar conta
            </Button>
            <Button variant="outline" className="w-full">
              Entrar com Google
            </Button>
          </form>
          <div className="mt-4 text-center text-sm">
            Já possui uma conta?{" "}
            <Link href="/auth/login" className="underline cursor-pointer">
              Entrar
            </Link>
          </div>
        </div>
      </div>
      <div className="hidden bg-muted lg:block">
        <div className="flex h-full items-center justify-center">
          <FileTransfer />
        </div>
      </div>
    </div>
  );
};

export default CreateAccount;
