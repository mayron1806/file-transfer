'use client';
import { createContext, useContext, useState } from "react";

type CookiesClientProps = {
  cookies: { [key: string]: string };
}
const CookiesClient = createContext<CookiesClientProps>({} as CookiesClientProps);

type Props = {
  children: React.ReactNode;
  cookies: { [key: string]: string };
}
const CookiesClientProvider = ({ children, cookies: c }: Props) => {
  const [cookies] = useState(c);
  return (
    <CookiesClient.Provider value={{ cookies: c }}>
      {children}
    </CookiesClient.Provider>
  );
}

export const useCookies = () => {
  return useContext(CookiesClient);
}
export default CookiesClientProvider;