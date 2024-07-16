import { z } from "zod";

const passwordLength = z.string().min(6, { message: "A senha deve ter pelo menos 6 caracteres" });
const passwordMaxLength = z.string().max(100, { message: "A senha deve ter no máximo 100 caracteres" });

const passwordUpperCase = z.string().regex(/[A-Z]/, { message: "A senha deve conter pelo menos uma letra maiúscula" });

const passwordLowerCase = z.string().regex(/[a-z]/, { message: "A senha deve conter pelo menos uma letra minúscula" });

const passwordNumber = z.string().regex(/\d/, { message: "A senha deve conter pelo menos um número" });

const passwordSpecialChar = z.string().regex(/[!@#$%^&*]/, { message: "A senha deve conter pelo menos um caractere especial (!@#$%^&*)" });

// Combine todos os critérios
export const passwordSchema = z.string()
  .superRefine((val, ctx) => {
    passwordLength.parse(val);
    passwordUpperCase.parse(val);
    passwordLowerCase.parse(val);
    passwordNumber.parse(val);
    passwordSpecialChar.parse(val);
    passwordMaxLength.parse(val);
  });