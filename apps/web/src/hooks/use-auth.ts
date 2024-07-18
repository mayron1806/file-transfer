import { useCookies } from "@/context/cookies/cookies-client-provider";
import { fetcher } from "@/lib/fetcher";
import { User } from "@/types/api/user";
import { useEffect, useState } from "react";
import useSWR from "swr";

type AuthRes = {
  accessToken?: string;
  user?: User;
  error: Error | null;
  isLoading: boolean;
}

export const useAuth = (): AuthRes => {
  const { cookies } = useCookies();
  const [accessToken, setAccessToken] = useState<string | undefined>(cookies['accessToken'] ?? undefined);
  
  useEffect(() => {
    setAccessToken(cookies['accessToken']);
  }, [cookies]);

  const { data, error, isLoading } = useSWR<User>('/api/user/me', fetcher(accessToken), {
    dedupingInterval: 10000,
    revalidateOnFocus: false,
    revalidateOnMount: false
  });
  
  return {
    accessToken,
    user: data,
    error,
    isLoading
  };
}