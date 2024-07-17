'use client'

import { InputWithLabel } from "@/components/input-with-label";
import { Card, CardHeader, CardTitle, CardContent, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import ErrorMessage from "@/components/error-message";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { ResetPasswordSchema, resetPasswordSchema } from "@/validation/reset-password";
import { ApiErrorResponse } from "@/types/api-error-response";
import { useToast } from "@/components/ui/use-toast";


const ResetPassword = ({ searchParams }: { searchParams: { token: string }}) => {
  const { toast } = useToast();
  const { register, handleSubmit, formState: { errors, isSubmitting }} = useForm<ResetPasswordSchema>({
    resolver: zodResolver(resetPasswordSchema),
  })
  const onSubmit = handleSubmit(async (data) => {
    const resquest = await fetch(`/api/v1/auth/reset-password`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });
    const res = await resquest.json();
    if (!resquest.ok) {
      const { error } = res as ApiErrorResponse<ResetPasswordSchema>;
      if (error) {
        toast({
          title: 'Erro',
          description: error,
          variant: 'destructive',
        })
      }
    }
    toast({
      title: 'Sucesso',
      description: "Senha alterada com sucesso.",
    })
  })
  return ( 
    <div className="w-screen h-screen flex items-center justify-center">
      <form className="min-w-80" onSubmit={onSubmit}>
        <Card>
          <CardHeader>
            <CardTitle>
              Alterar senha
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            <input type="hidden" name="token" value={searchParams.token} />
            <InputWithLabel {...register('password')} type="password" placeholder="Senha" label="Senha: *" error={errors?.password?.message} />
            <InputWithLabel {...register('confirm-password')} type="password" placeholder="Confirmar senha" label="Confirmar senha: *" error={errors?.password?.message} />
          </CardContent>
          <CardFooter className="flex flex-col">
            <ErrorMessage>{errors?.root?.message}</ErrorMessage>
            <Button loading={isSubmitting} className="mb-2">Alterar senha</Button>
            <a href="/auth/login" aria-disabled={isSubmitting}>Voltar para login</a>
          </CardFooter>
        </Card>
      </form>
    </div>
  );
}
export default ResetPassword;