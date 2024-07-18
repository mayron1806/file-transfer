import { env } from "@/env";
import { Organization } from "@/types/api/organization";
import { ServiceResponse } from "./types";
import cache from "@/lib/cache";

export const getOrganization = async (organizationId: number, accessToken: string): Promise<ServiceResponse<Organization>> => {
  const cachedData = cache.get(`organization-${organizationId}-${accessToken}`);
  if (cachedData) return cachedData as ServiceResponse<Organization>;
  
  const res = await fetch(`${env.API_URL}/organization/${organizationId}`, {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${accessToken}`
    },
    cache: 'no-store'
  });
  const data = await res.json();
  cache.set(`organization-${organizationId}-${accessToken}`, { success: res.ok, data: data, error: { message: data.error, status: res.status } });
  if (!res.ok) return { success: false, error: { message: data.error, status: res.status } };
  
  return { success: true, data };
}