import { File } from "../file";
import { Organization } from "../organization";

export type Transfer = {
  id: number;
  key: string;
  name: string;
  files: File[];
  organizationId: number;
  organization?: Organization;
  createdAt: Date;
  expiresAt: Date;
  size: number;
  filesCount: number;
  path: string;
  password: string;
  emailsDestination: string[];
  expiresOnDownload: boolean;
  expiresOnDownloadCount: number;
  expiresOnDownloadSize: number;

}