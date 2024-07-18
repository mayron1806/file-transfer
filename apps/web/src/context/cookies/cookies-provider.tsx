import { cookies } from "next/headers";
import CookiesClientProvider from "./cookies-client-provider";

const CookiesProvider = ({ children }: { children: React.ReactNode }) => {
  const allCookies = cookies().getAll(); 
  return (
    <CookiesClientProvider cookies={Object.entries(allCookies).reduce((acc, [k, v]) => ({ ...acc, [v.name]: v.value }), {})}>
      {children}
    </CookiesClientProvider>
  );
}
 
export default CookiesProvider;