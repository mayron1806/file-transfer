import { NextRequest, NextResponse } from "next/server";
import { UserAuth } from "./types/api/user";
import { env } from "./env";
import { cookies } from "next/headers";
import moment from "moment";
import { isJwtValid } from "./lib/is-jwt-valid";

const handleRefreshToken = async (refreshToken: string): Promise<UserAuth> => {
  const req = await fetch(`${env.API_URL}/user/refresh-token`, {
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });
  if (!req.ok) {
    const res = await req.json();
    throw new Error(res.error);
  }
  const res = await req.json();
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
    (refreshToken && accessToken && !await isJwtValid(accessToken)) || // accessToken invalido ou expirado
    (!accessToken && refreshToken) || // Se não há accessToken mas há refreshToken
    (refreshToken && expiresAt && moment().isAfter(moment(expiresAt))) // Se o accessToken expirou
  ) {
    console.log("refresh token");
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
      console.log('refreshed token');
      
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
