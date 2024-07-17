'use server'
import jwt from 'jsonwebtoken';
import { prisma } from '@/lib/prisma';
import z from 'zod';
import { sendEmail } from '@/lib/mail';
import { env } from '@/env';
const forgotPasswordSchema = z.object({
  email: z.string().email({ message: 'Email inválido' }),
});

export const forgotPassword = async (prevState: any, formData: FormData) => {
  const validationResult = forgotPasswordSchema.safeParse({
    email: formData.get('email'),
  });
  if (!validationResult.success) {
    return { error: validationResult.error.flatten().fieldErrors }
  }
  const userWithEmail = await prisma.user.findFirst({
    where: {
      email: validationResult.data.email,
    },
  });
  if (!userWithEmail) return { errorMessage: 'Usurário não encontrado' };
  const token = jwt.sign({ email: userWithEmail.email, id: userWithEmail.id }, env.JWT_SECRET, { expiresIn: '5m' });
  await sendEmail({ 
    to: userWithEmail.email, 
    subject: "Alteração de senha",
    text:`${env.BASE_URL}/auth/reset-password?token=${token}`
  });
  return {
    success: 'Um link de alteração de senha foi enviado para seu e-mail',
  }
} 