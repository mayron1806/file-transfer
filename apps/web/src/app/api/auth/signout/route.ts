import { cookies } from "next/headers"
import { NextResponse } from "next/server";

export const POST = async () => {
  cookies().delete('accessToken');
  cookies().delete('refreshToken');
  cookies().delete('expiresAt');

  return NextResponse.json({ success: true });
}