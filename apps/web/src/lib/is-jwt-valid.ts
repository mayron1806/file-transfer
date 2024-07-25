import { env } from '@/env';
export const isJwtValid = async (token: string): Promise<boolean> => {
  return new Promise(async (resolve) => {
    try {
      const secret = await crypto.subtle.importKey(
        "raw",
        new TextEncoder().encode(env.JWT_SECRET),
        {
          name: "HMAC",
          hash: { name: "SHA-256" },
        },
        false,
        ["verify"]
      );
    
      const result = await crypto.subtle.verify(
        "HMAC",
        secret,
        Buffer.from(token.split(".")[2], "base64url"),
        new TextEncoder().encode(token.split(".").splice(0, 2).join("."))
      );
      resolve(result);
    } catch (error) {
      resolve(false);
    }
  });
}