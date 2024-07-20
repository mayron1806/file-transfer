export type Organization = {
  id: number;
  plan: Plan;
  planActive: boolean;
  createdAt: Date;
  storedSize: string;
  dayUploadCount: number;
}
export type Plan = {
  name: string;
  price: number;
  description: string;
  limits: PlanLimits;
}
export type PlanLimits = {
  maxUploadSize: number;
  maxStorageSize: number;
  maxEmails: number;
  canUseExpiresOnDownload: boolean;
  canUsePassword: boolean;
  maxExpireDays: number;
  maxUploadConcurrency: number;
  maxUploadPerDay: number; 
}