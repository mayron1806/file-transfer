'use client';

import { Organization } from "@/types/api/organization";
import { createContext, useContext, useState } from "react";

type OrganizationContextProps = {
  organization: Organization;
  setOrganization: (organization: Organization) => void;
}  
const OrganizationContext = createContext<OrganizationContextProps>({} as OrganizationContextProps);
type Props = {
  children: React.ReactNode;
  organization: Organization;
}
const OrganizationProvider = ({ children, organization }: Props) => {
  const [org, setOrg] = useState(organization);
  return (
    <OrganizationContext.Provider value={{ organization: org, setOrganization: setOrg }}>
      {children}
    </OrganizationContext.Provider>
  );
}
export const useOrganization = () => {
  return useContext(OrganizationContext);
}
export default OrganizationProvider;