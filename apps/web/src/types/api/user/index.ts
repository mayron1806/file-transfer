export type UserAuth = {
  accessToken: string;
  refreshToken: string;
  expires: Date;
}
export type User = {
  id: number;
  email: string;
  name: string;
}