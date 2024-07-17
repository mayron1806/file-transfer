import { UserAuth } from "@/types/api/user";
import moment from "moment";
import { cookies } from "next/headers";
import { NextResponse } from "next/server";

export const POST = async (req: Request) => {
  const { email, password } = await req.json();
  console.log('here');
  
  const res = await fetch(`${process.env.API_URL}/user/login`, {
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
  const data = await res.json();
  console.log(data);
  
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
  return NextResponse.json(data);
}