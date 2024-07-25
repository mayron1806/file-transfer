import { z } from "zod"

const envSchema = z.object({
  API_URL: z.string(),
  BASE_URL: z.string(),
  SECURE_COOKIES: z.enum(['true', 'false']).refine(v => v === 'true' || v === 'false').transform(v => v === 'true'),
  NODE_ENV: z.enum(['development', 'test', 'production']),
  JWT_SECRET: z.string(),
});
export const env = envSchema.parse(process.env);