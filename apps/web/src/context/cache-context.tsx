'use client'
import { createContext, useContext } from "react";
type Cache = {
  get: <T>(key: string) => T | undefined,
  set: <T>(key: string, value: T) => void,
  delete: (key: string) => void,
  clear: () => void
};

const defaultCache = new Map<string, any>() as Cache;
type CacheContextProps = {
  cache: Cache;
}
const CacheContext = createContext<CacheContextProps>({ cache: defaultCache });
type Props = {
  cache?: Cache;
  children: React.ReactNode
}

const CacheProvider = ({ cache = defaultCache, children }: Props) => {
  return (
    <CacheContext.Provider value={{ cache }}>
      {children}
    </CacheContext.Provider>
  );
}
 
export const useCache = () => {
  return useContext(CacheContext).cache
}
export default CacheProvider;


