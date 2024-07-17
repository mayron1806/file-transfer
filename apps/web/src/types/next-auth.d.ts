import NextAuth, { User, type DefaultSession } from 'next-auth';

declare module "next-auth" {
  interface Session {
    user: {
      accessToken: string;
      refreshToken: string;
      expiresAt: number;
    };
  }
  interface User {
    expiresAt: number;
    accessToken: string;
    refreshToken: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    user: {
      expiresAt: number;
      accessToken: string;
      refreshToken: string;
    };
  }
}