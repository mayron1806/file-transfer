import EventProvider from "@/context/event";
import { ThemeProvider } from "@/context/theme";
import { Outlet } from "react-router-dom";

const RootLayout = () => {
  return (
    <div>
      <ThemeProvider>
          <Outlet />
      </ThemeProvider>
    </div>
  );
}
 
export default RootLayout;