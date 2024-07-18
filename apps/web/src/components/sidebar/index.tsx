import { Bell, DownloadCloud, Package2, UploadCloud } from "lucide-react";
import { Button } from "@/components/ui/button";
import UpgradeCard from "../upgrate-plan-card";
import NavItem from "./nav-item";
type Props = {
  organizationId: number;
}
const Sidebar = ({ organizationId }: Props) => {
  return (
    <div className="hidden border-r bg-muted/40 md:block">
      <div className="flex h-full max-h-screen flex-col gap-2">
        <div className="flex h-14 items-center border-b px-4 lg:h-[60px] lg:px-6">
          <a href={`/dashboard/${organizationId}`} className="flex items-center gap-2 font-semibold">
            <Package2 className="h-6 w-6" />
            <span>Acme Inc</span>
          </a>
          <Button variant="outline" size="icon" className="ml-auto h-8 w-8 rounded-full">
            <Bell className="h-4 w-4" />
            <span className="sr-only">Toggle notifications</span>
          </Button>
        </div>
        <div className="flex-1">
          <nav className="grid items-start px-2 text-sm font-medium lg:px-4">
            <NavItem href={`/dashboard/${organizationId}/send-files`}>
              <UploadCloud className="h-4 w-4" />
              Enviar arquivos
            </NavItem>
            <NavItem href={`/dashboard/${organizationId}/receive-files`}>
              <DownloadCloud className="h-4 w-4" />
              Receber arquivos
            </NavItem>
          </nav>
        </div>
        <div className="mt-auto p-4">
          <UpgradeCard />
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
