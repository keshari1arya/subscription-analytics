import { z } from 'zod'

export const waitlistSchema = z.object({
  email: z.string().min(1, 'Email is required').email('Please enter a valid email address'),
  source: z.string().optional(),
  metadata: z.object({}).optional(),
})

export const contactSchema = z.object({
  firstName: z.string().min(1, 'First name is required'),
  lastName: z.string().min(1, 'Last name is required'),
  email: z.string().min(1, 'Email is required').email('Please enter a valid email address'),
  company: z.string().optional(),
  message: z.string().min(10, 'Message must be at least 10 characters'),
  metadata: z.object({}).optional(),
})

export type WaitlistInput = z.infer<typeof waitlistSchema>
export type ContactInput = z.infer<typeof contactSchema>
