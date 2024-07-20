import { env } from "@/env";
import { Organization } from "@/types/api/organization";
import cache from "@/lib/cache";
import { HttpError } from "@/errors/HttpError";

export const getOrganization = async (organizationId: number, accessToken: string): Promise<Organization> => {
  const cachedData = cache.get(`organization-${organizationId}-${accessToken}`);
  if (cachedData) return cachedData as Organization;
  
  const res = await fetch(`${env.API_URL}/organization/${organizationId}`, {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${accessToken}`
    },
    cache: 'no-store'
  });
  const data = await res.json();
  
  if (!res.ok) throw new HttpError(data.error, res.status);
  cache.set(`organization-${organizationId}-${accessToken}`,data);
  return data as Organization;
}