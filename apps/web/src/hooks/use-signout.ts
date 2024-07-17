'use client';
import { signOut } from "next-auth/react"
export const useSignOut = () => {
  return { signOut: () => signOut({ redirect: true }) };
}