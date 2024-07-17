'use client'
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import FileTransfer from "@/assets/file-transfer.svg";
import Title from "@/components/title";
import Description from "@/components/description";
import { useToast } from "@/components/ui/use-toast";
import { LoginSchema, loginSchema } from "@/validation/login";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import Link from "next/link";
import { useRouter } from "next/navigation";

const Login = () => {
  const { toast } = useToast();
  const router = useRouter();
  const { register, handleSubmit, formState: { errors, isSubmitting }, setError } = useForm<LoginSchema>({
    resolver: zodResolver(loginSchema)
  });
  const submit = handleSubmit(async (body) => {
    try {
      console.log(body);
      
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
      });
      const data = await res.json();
      console.log(data);
      
      if (!res?.ok) {
        setError('root', { message: data.error ?? 'Ocorreu um erro ao fazer login' });
        toast({
          title: 'Erro ao fazer login',
          description: data.error ?? 'Ocorreu um erro ao fazer login',
          variant: 'destructive',
        });
        return;
      }
      console.log(data);
      
      return router.push('/');
    } catch (error) {
      if(error instanceof Error) {
        setError('root', { message: error.message });
      }
    }
  });
  return (
    <div className="w-full lg:grid h-screen lg:min-h-[600px] lg:grid-cols-2 xl:min-h-[800px]">
      <div className="flex items-center justify-center py-12">
        <div className="mx-auto grid w-[350px] gap-6">
          <div className="grid gap-2 text-center">
            <Title>Login</Title>
            <Description>
              Faça login para utilizar a sua conta
            </Description>
          </div>
          <form onSubmit={submit} className="grid gap-4">
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
                <a
                  href="/forgot-password"
                  className="ml-auto inline-block text-sm underline"
                >
                  Esqueceu sua senha?
                </a>
              </div>
              <Input {...register('password')} type="password" required />
            </div>
            <Button
              type="submit"
              className="w-full"
              loading={isSubmitting}
              disabled={isSubmitting}
            >
              Login
            </Button>
            <Button variant="outline" className="w-full">
              Entrar com Google
            </Button>
          </form>
          <div className="mt-4 text-center text-sm">
            Ainda não tem uma conta?{" "}
            <Link href="/auth/create-account" className="underline">
              Criar conta
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

export default Login;
