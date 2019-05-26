export class ApiResponse<T> {
  isSuccess: boolean;
  data: T;

  public static extractData<T>(response: ApiResponse<T>): T {
    return response.data;
  }
}
