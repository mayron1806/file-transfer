import { z } from "zod"

export const sendFilesSchema = z.object({
  files: typeof window === 'undefined' ? z.any() : z.instanceof(FileList).refine(arg => arg.length > 0, { message: "Selecione pelo menos um arquivo" }),
  name: z.string().max(100, 'O nome deve ter no maximo 100 caracteres').optional(),
  message: z.string().max(2000, 'Mensagem deve ter no maximo 2000 caracteres').optional(),
  password: z.string().max(50, 'Senha deve ter no maximo 50 caracteres').optional(),
  expiresOnDownload: z.boolean().optional(),
  emailsDestination: z.array(z.string().email('E-mail invalido')).optional(),
  expiresAt: z.date().optional(),
});

export type SendFilesSchema = z.infer<typeof sendFilesSchema>;