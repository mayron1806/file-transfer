import { UserAuth } from "@/types/api/user";
import moment from "moment";
import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export const POST = async (req: Request) => {
  const { email, password } = await req.json();
  
  const res = await fetch(`${process.env.API_URL}/user/login`, {
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
  const data = await res.json();
  if (!res.ok) {
    return new Response(JSON.stringify(data), { status: 401 });
  }
  
  const { accessToken, expires, refreshToken } = data as UserAuth;
  cookies().set('accessToken', accessToken, {
    expires: new Date(expires),
    httpOnly: true,
    sameSite: 'lax',
    path: '/',
  });
  cookies().set('refreshToken', refreshToken, {
    httpOnly: true,
    sameSite: 'lax',
    path: '/',
  });
  cookies().set('expiresAt', moment(expires).toISOString(), {
    httpOnly: true,
    sameSite: 'lax',
    path: '/',
  });
  console.log('here login');
  
  let organization;
  const orgRes = await fetch(`${process.env.API_URL}/organization`, {
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${accessToken}`,
    },
    method: "GET",
  });
  if (res.ok) organization = await orgRes.json();
  else if (orgRes?.status === 404) {
    const newOrganization = await fetch(`${process.env.API_URL}/organization`, {
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${accessToken}`,
      },
      method: "POST",
    });
    const data = await newOrganization.json();
    if (!newOrganization.ok) return new Response(JSON.stringify(data), { status: newOrganization.status });
    organization = data;
  }
  return NextResponse.json({ destination: `/dashboard/${organization.id}` });
}