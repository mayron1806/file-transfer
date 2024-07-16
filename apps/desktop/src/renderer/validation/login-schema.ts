import { z } from "zod";

export const loginSchema = z.object({
  email: z
    .string()
    .min(1, "O campo email é obrigatório")
    .email("O campo email deve ser um email valido"),
  password: z
    .string()
    .min(1, "O campo senha é obrigatório")
});
export type LoginSchema = z.infer<typeof loginSchema>;