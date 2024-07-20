import { redirect } from "next/navigation";

export const signOut = async () => {
  const response = await fetch('/api/auth/signout', {
    method: 'POST',
  });
  const data = await response.json();
  if (!response.ok) {
    throw new Error(data.error);
  }
  redirect('/auth/login');
};