import { z } from "zod";
import { passwordSchema } from "./password";

export const resetPasswordSchema = z.object({
  token: z.string().min(1, 'Token obrigatório'),
  password: passwordSchema,
  ['confirm-password']: passwordSchema,
}).superRefine((arg, ctx) => {
  if (arg.password !== arg['confirm-password']) {
    ctx.addIssue({
      path: ['root'],
      code: z.ZodIssueCode.custom,
      message: `As senhas não coincidem.`,
    });
  }
});
export type ResetPasswordSchema = z.infer<typeof resetPasswordSchema>;