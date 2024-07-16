import { z } from "zod";
import { passwordSchema } from "./password";

export const createAccountSchema = z.object({
  name: z.string().min(3, 'Nome deve ter ao menos 3 caracteres'),
  email: z.string().email('E-mail invalido'),
  password: passwordSchema,
  "confirm-password": passwordSchema,
}).superRefine((arg, ctx) => {
  if (arg.password !== arg["confirm-password"]) {
    ctx.addIssue({
      path: ['confirm-password'],
      code: z.ZodIssueCode.custom,
      message: `As senhas não coincidem.`,
    });
  }
});
export type CreateAccountSchema = z.infer<typeof createAccountSchema>;