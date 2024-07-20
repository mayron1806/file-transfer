type ErrorResponse = {
  success: false;
  error: { message: string, status: number };
}
type SuccessResponse<T> = {
  success: true;
  data: T;
}
export type Response<T> = ErrorResponse | SuccessResponse<T>;