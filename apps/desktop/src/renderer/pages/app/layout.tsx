import Header from "@/components/header";
import Sidebar from "@/components/sidebar";
import { Outlet } from "react-router-dom";

const AppLayout = () => {
  return ( 
    <div className="grid min-h-screen w-full md:grid-cols-[220px_1fr] lg:grid-cols-[280px_1fr]">
      <Sidebar />
      <div className="flex flex-col">
        <Header />
        <main className="flex flex-1 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
 
export default AppLayout;