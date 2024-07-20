import { NextRequest, NextResponse } from "next/server";
import { UserAuth } from "./types/api/user";
import { env } from "./env";
import { cookies } from "next/headers";
import moment from "moment";

const handleRefreshToken = async (refreshToken: string): Promise<UserAuth> => {
  const req = await fetch(`${env.API_URL}/user/refresh-token`, {
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });
  const res = await req.json();
  if (!req.ok) {
    throw new Error(res.error);
  }
  return res as UserAuth;
}

export default async function middleware(req: NextRequest) {
  if (req.nextUrl.pathname.startsWith("/_next")) {
    return NextResponse.next();
  }
  
  console.log("start middleware");
  
  const accessToken = cookies().get('accessToken')?.value;
  const expiresAt = cookies().get('expiresAt')?.value;
  const refreshToken = cookies().get('refreshToken')?.value;

  // Redirecionar imediatamente se não houver tokens
  if (!accessToken && !refreshToken) {
    console.log("redirect to login");
    return NextResponse.redirect(new URL('/auth/login', req.url));
  }

  
  if (
    !accessToken && refreshToken || // Se não há accessToken mas há refreshToken, renovar o accessToken
    refreshToken && expiresAt && moment().isAfter(moment(expiresAt)) // Se o accessToken expirou, renovar o accessToken
  ) {
    try {
      const userAuth = await handleRefreshToken(refreshToken);
      const response = NextResponse.next();
      response.cookies.set('accessToken', userAuth.accessToken, {
        httpOnly: true,
        sameSite: 'lax',
        path: '/',
      });
      response.cookies.set('refreshToken', userAuth.refreshToken, {
        httpOnly: true,
        sameSite: 'lax',
        path: '/',
      });
      response.cookies.set('expiresAt', moment(userAuth.expires).toISOString(), {
        httpOnly: true,
        sameSite: 'lax',
        path: '/',
      });
      return response;
    } catch (error) {
      console.error('Error refreshing token:', error);
      return NextResponse.redirect(new URL('/auth/login', req.url));
    }
  }

  return NextResponse.next();
}

// Seleciona as rotas privadas
export const config = {
  matcher: [
    "/",
    "/((?!auth|api/auth).*)", // todas as rotas, exceto as que iniciam com /auth ou /api/auth
  ],
};
