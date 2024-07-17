'use client';

import { cn } from "@/lib/utils";
import { usePathname } from "next/navigation";

type Props = {
  children: React.ReactNode;
  href: string;
}
const NavItem = ({ children, href }: Props) => {
  const pathname = usePathname();
  return ( 
    <a href={href} className={
      cn("flex items-center gap-3 rounded-lg px-3 py-2 text-muted-foreground transition-all hover:text-primary hover:opacity-70"
      ,pathname === href && "text-primary"
      )}>
      {children}
    </a>
  );
}
 
export default NavItem;