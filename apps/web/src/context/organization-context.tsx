'use client';

import { Organization } from "@/types/api/organization";
import { createContext, useContext, useState } from "react";

type OrganizationContextProps = {
  organization: Organization;
}  
const OrganizationContext = createContext<OrganizationContextProps>({} as OrganizationContextProps);
type Props = {
  children: React.ReactNode;
  organization: Organization;
}
const OrganizationProvider = ({ children, organization }: Props) => {
  const [org] = useState(organization);
  return (
    <OrganizationContext.Provider value={{ organization: org }}>
      {children}
    </OrganizationContext.Provider>
  );
}
export const useOrganization = () => {
  return useContext(OrganizationContext);
}
export default OrganizationProvider;