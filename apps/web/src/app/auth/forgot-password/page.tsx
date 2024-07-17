'use client'
import { InputWithLabel } from "@/components/input-with-label";
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardContent, CardFooter, CardDescription } from "@/components/ui/card";
import { forgotPassword } from "./action";
import Link from "@/components/link";
import ErrorMessage from "@/components/error-message";
import { useFormState, useFormStatus } from "react-dom";

const ForgotPassword = () => {
  const [state, action] = useFormState(forgotPassword, null);
  const { pending } = useFormStatus();
 
  return (
    <div className="w-screen h-screen flex items-center justify-center">
      <form action={action} className="min-w-80">
        <Card>
          <CardHeader>
            <CardTitle>
              Esqueci minha senha
            </CardTitle>
            <CardDescription>
              Caso tenha esquecido sua senha preencha o campo abaixo com o email da sua conta, um email será enviado para ele com um link, onde você pode alterar sua senha.
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
            <InputWithLabel name="email" error={state?.error?.email?.[0]} autoComplete="email" placeholder="E-mail" label="E-mail: *" />
          </CardContent>
          <CardFooter className="flex flex-col gap-2">
            <ErrorMessage>{state?.errorMessage}</ErrorMessage>
            {
              state?.success && <p className="text-foreground text-sm text-green-400">{state?.success}</p>
            }
            <Button loading={pending} >Enviar e-mail</Button>
            <Link href="/auth/login" aria-disabled={pending}>Voltar para login</Link>
          </CardFooter>
        </Card>
      </form>
    </div>  
  );
}
 
export default ForgotPassword;