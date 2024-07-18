
export interface SWRError extends Error {
  status: number;
}
type FetcherFn = <T>(accessToken?: string) => (input: RequestInfo, init?: RequestInit) => Promise<T>;

export const fetcher: FetcherFn = <T>(accessToken?: string)=> {
  return async (input: RequestInfo, init?: RequestInit) => {
    const res = await fetch(input,
      {
        ...init,
        headers: {
          ...init?.headers,
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': accessToken ? `Bearer ${accessToken}` : '',
        },
      }
    );
  
    const data = await res.json();
    if (!res.ok) {
      const err = new Error(data.error) as SWRError;
      err.status = res.status;
      throw err;
    }
  
    return data as T;
  }
}