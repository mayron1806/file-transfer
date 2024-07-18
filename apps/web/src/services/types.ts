type ErrorServiceResponse = {
  success: false;
  error: { message: string, status: number };
}
type SuccessServiceResponse<T> = {
  success: true;
  data: T;
}
export type ServiceResponse<T> = ErrorServiceResponse | SuccessServiceResponse<T>;