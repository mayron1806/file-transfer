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

  if (!accessToken || !expiresAt || !refreshToken) {
    console.log("redirect to login");
    return NextResponse.redirect(new URL('/auth/login', req.url));
  }

  if (moment().isAfter(moment(expiresAt))) {
    const userAuth = await handleRefreshToken(refreshToken);
    cookies().set('accessToken', userAuth.accessToken, {
      expires: new Date(userAuth.expires),
      httpOnly: true,
      sameSite: 'lax',
      path: '/',
    });
    cookies().set('refreshToken', userAuth.refreshToken, {
      httpOnly: true,
      sameSite: 'lax',
      path: '/',
    });
    cookies().set('expiresAt', moment(userAuth.expires).toISOString(), {
      httpOnly: true,
      sameSite: 'lax',
      path: '/',
    });
  }
  return NextResponse.next();
}
// seleciona as rotas privadas
export const config = {
  matcher: [
    "/",
    "/((?!auth|api/auth).*)", // todas as rotas, exceto as que iniciam com /auth ou /api/auth
  ],
};
