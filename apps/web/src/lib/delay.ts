import { env } from "@/env"

export const delay = (ms: number) => {
  if (env.NODE_ENV !== 'production') return new Promise((resolve) => setTimeout(resolve, ms));
}