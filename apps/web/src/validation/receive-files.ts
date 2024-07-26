import { allowedFiles } from "@/constraints/allowed-files";
import { z } from "zod";


export const receiveFilesSchema = z.object({
  name: z.string().max(100, 'O nome deve ter no máximo 100 caracteres').optional(),
  message: z.string().max(2000, 'Mensagem deve ter no máximo 2000 caracteres').optional(),
  maxSize: z.number().optional(),
  maxFiles: z.number().optional(),
  acceptedFiles: z.array(z.string().refine(ext => allowedFiles.includes(ext), { message: 'Tipo de arquivo não permitido' })).optional(),
  expiresAt: z.date().optional(),
  password: z.string().max(50, 'Senha deve ter no máximo 50 caracteres').optional(),
  advancedSettings: z.boolean().optional(),
});

export type ReceiveFilesSchema = z.infer<typeof receiveFilesSchema>;
