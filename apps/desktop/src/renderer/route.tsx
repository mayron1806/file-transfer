import { MemoryRouter, Routes, Route } from 'react-router-dom';

import "tailwindcss/tailwind.css";
import './App.css';
import SignIn from './pages/signin';
import CreateAccount from './pages/create-account';
import Index from './pages/index';
import UploadFiles from './pages/upload-files';

export default function Router() {
  return (
    <MemoryRouter>
      <Routes>
        <Route path="/" element={<Index />} />
        <Route path="/signin" element={<SignIn />} />
        <Route path="/create-account" element={<CreateAccount />} />
        <Route path="/upload-files" element={<UploadFiles />} />
      </Routes>
    </MemoryRouter>
  );
}
