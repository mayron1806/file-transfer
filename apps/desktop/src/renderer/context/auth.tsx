import { useLocalStorage } from '@/hooks/useLocalStorage';
import React, { createContext } from 'react';
import { Auth } from 'src/types/auth';

type AuthContextProps = {
  auth: Auth;
  handleSignup: (name: string, email: string, password: string) => Promise<void>;
  handleLogin: (email: string, password: string) => Promise<void>;
  handleLogout: () => void;
  isAuthenticated: boolean;
}
const AuthContext = createContext<AuthContextProps>({} as AuthContextProps);

const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [auth, setAuth] = useLocalStorage<Auth>('auth', null);
  const handleSignup = async (name: string , email: string, password: string) => {
  
  };

  const handleLogin = async (email: string, password: string) => {
  };
  const handleLogout = () => {
    setAuth(null);
  };
  const isAuthenticated = !!auth;
  return (
    <AuthContext.Provider value={{ auth, handleSignup, handleLogin, handleLogout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};
export const useAuth = () => React.useContext(AuthContext);
export { AuthProvider };
