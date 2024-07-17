import LottieError from "@/components/lottie/error";
import LottieSuccess from "@/components/lottie/success";
import Title from "@/components/title";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { env } from "@/env";
import Link from "next/link";
const handleActiveAccount = async (token: string) => {
  const res = await fetch(`${env.API_URL}/user/active-account`, {
    method: 'POST',
    body: JSON.stringify({ token }),
    headers: {
      'Content-Type': 'application/json',
    },
    cache: 'no-cache'
  });  
  const data = await res.json();
  if (!res.ok) {
    return { error: data.error };
  }
  return { success: true };
}
const ActiveAccount = async ({ token }: { token: string}) => {
  const result = await handleActiveAccount(token);
  
  if (!result.success) {
    return (
      <div className="container h-screen flex flex-col items-center justify-center">
        <Card className="">
          <CardHeader>
            <CardTitle className="text-center text-green-500">Conta ativada com sucesso</CardTitle>
          </CardHeader>
          <CardContent className="">
            <LottieSuccess />
          </CardContent>
          <CardFooter className="flex flex-col gap-2">
            <Link className="underline" href="/auth/login">Ir para login</Link>
          </CardFooter>
        </Card>
      </div>
    )
  }

  return (
    <div className="container h-screen flex flex-col items-center justify-center">
      <Card className="">
        <CardHeader>
          <CardTitle className="text-center text-red-500">{result.error}</CardTitle>
        </CardHeader>
        <CardContent className="">
          <LottieError />
        </CardContent>
        <CardFooter className="flex flex-col gap-2">
          <Link className="underline" href="/auth/login">Voltar para login</Link>
        </CardFooter>
      </Card>
    </div>
  )
}
 
export default ActiveAccount;