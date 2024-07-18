'use client';

import { useRouter } from "next/navigation";

export const useSignOut = () => {
  const router = useRouter();
  return { 
    signOut: async () => {
      const response = await fetch('/api/auth/signout', {
        method: 'POST',
      });
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data.error);
      }
      router.push('/auth/login');
    } 
  };
}