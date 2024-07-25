'use client';
import { ThemeProvider } from "./theme-provider";
import { Toaster } from "@/components/ui/toaster";
import CacheProvider from "@/context/cache-context";
import QueryProvider from "@/context/query";
const Providers = ({ children }: {children: React.ReactNode}) => {
  return ( 
    <ThemeProvider
      attribute="class"
      defaultTheme="system"
      enableSystem
      disableTransitionOnChange
    >
      <QueryProvider>
        <CacheProvider>
            {children}
        </CacheProvider>
      </QueryProvider>
      <Toaster />
    </ThemeProvider>
  );
}
 
export default Providers;