import { createRoot } from 'react-dom/client';
import { MemoryRouter, Routes, Route, RouteProps, Navigate, Outlet } from 'react-router-dom';

import "../index.css";
import Index from './pages/index/index';
import EventProvider from './context/event';
import SignIn from './pages/auth/signin';
import CreateAccount from './pages/auth/create-account';
import ForgetPassword from './pages/auth/forget-password';
import ReceiveFiles from './pages/app/receive-files';
import SendFiles from './pages/app/send-files';
import Account from './pages/auth/account';
import ActiveAccount from './pages/auth/active-account';
import { ThemeProvider } from './context/theme';
import AppLayout from './pages/app/layout';
import { AuthProvider, useAuth } from './context/auth';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/auth/signin" />
}

const container = document.getElementById("root");
const root = createRoot(container);
root.render(
  <MemoryRouter>
    <AuthProvider>
      <ThemeProvider attribute='class' defaultTheme='system' enableSystem disableTransitionOnChange>
        <EventProvider>
          <Routes>
            <Route path="/" element={<Index />} />
            <Route path="/auth">
              <Route path="signin" element={<SignIn />} />
              <Route path="active-account" element={<ActiveAccount />} />
              <Route path="create-account" element={<CreateAccount />} />
              <Route path="forget-password" element={<ForgetPassword />} />
            </Route>
            <Route path='/app' element={
              <PrivateRoute>
                <AppLayout />
              </PrivateRoute>
            }>
              <Route path="receive-files" element={<ReceiveFiles />} />
              <Route path="send-files" element={<SendFiles />} />
              <Route path="account" element={<Account />} />
            </Route>
          </Routes>
        </EventProvider>
      </ThemeProvider>
    </AuthProvider>
  </MemoryRouter>
);
