import { File } from "../file";
import { Organization } from "../organization";
export type GetTransfers = {
  transfers: SimpleTransfer[];
}

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
export type SimpleTransfer = {
  id: number;
  key: string;
  name: string;
  createdAt: Date;
  expiresAt: Date;
  size: number;
  filesCount: number;
  type: "send" | "receive";
  send?: {
    hasPassword: boolean;
    downloads: number;
    expiresOnDownload: boolean;
  };
  receive?: {
    hasPassword: boolean;
  }
}