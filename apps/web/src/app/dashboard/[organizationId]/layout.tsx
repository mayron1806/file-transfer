import Header from "@/components/header";
import Sidebar from "@/components/sidebar";
import OrganizationProvider from "@/context/organization-context";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import Error from "@/components/error";
import { getOrganization } from "@/services/organization";
import { signOut } from "@/services/auth";
import { Organization } from "@/types/api/organization";
import { env } from "@/env";

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
      return <Error error={data.error} goBackAction={signOut} />
    }
    const { id } = data as Organization;
    redirect(`/dashboard/${id}`);
  }
  const organization = await getOrganization(params.organizationId, accessToken);
  if (!organization.success) return <Error error={organization.error} goBackAction={handleNavigateToDashboard}  goBackActionLabel="Ir para o meu painel" />

  return (
    <OrganizationProvider organization={organization.data}>
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