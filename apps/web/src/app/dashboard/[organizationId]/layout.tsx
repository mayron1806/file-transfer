import Header from "@/components/header";
import Sidebar from "@/components/sidebar";
import OrganizationProvider from "@/context/organization-context";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import ErrorScreen from "@/components/error";
import { Organization } from "@/types/api/organization";
import { env } from "@/env";
import { signOut } from "@/usecases/signout";
import { HttpError } from "@/errors/HttpError";
import { getOrganization } from "@/usecases/get-organization";

type Props = {
  children: React.ReactNode;
  params: { organizationId: number };
}
const AppLayout = async ({ children, params }: Props) => {
  const accessToken = cookies().get('accessToken')?.value;
  if (!accessToken) return redirect('/auth/login');
  const handleNavigateToDashboard = async () => {
    'use server';
    const res = await fetch(`${env.API_URL}/organization`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json'
      },
      cache: 'no-cache'
    });
    const data = await res.json();
    if (!res.ok) {
      return <ErrorScreen error={data.error} goBackAction={signOut} />
    }
    const { id } = data as Organization;
    redirect(`/dashboard/${id}`);
  }
  let organization: Organization;
  try {
    organization = await getOrganization(params.organizationId, accessToken);
  } catch (error) {
    if (error instanceof HttpError) {
      return <ErrorScreen error={{ message: error.message, status: error.status }} goBackAction={handleNavigateToDashboard}  goBackActionLabel="Ir para o meu painel" />
    }
    if (error instanceof Error) {
      return <ErrorScreen error={{ message: error.message, status: 400 }} goBackAction={handleNavigateToDashboard}  goBackActionLabel="Ir para o meu painel" />
    }
    return (
      <ErrorScreen 
        error={{
          message: "Ocorreu um erro ao buscar seus dados, faça login novamente para continuar", status: 400
        }} 
        goBackAction={signOut} 
      />
    );
  }

  return (
    <OrganizationProvider organization={organization!}>
      <div className="grid min-h-screen w-full md:grid-cols-[220px_1fr] lg:grid-cols-[280px_1fr]">
        <Sidebar organizationId={params.organizationId} />
        <div className="flex flex-col">
          <Header organizationId={params.organizationId} />
          <main className="flex flex-1 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
            {children}
          </main>
        </div>
      </div>
    </OrganizationProvider>
  );
}
 
export default AppLayout;