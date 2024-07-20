import { Transfer } from "../transfer";

export type File = {
  id: number;
  key: string;
  originalName: string;
  path: string;
  size: number;
  errorMessage?: string;
  contentType?: string;
  transferId?: string;
  transfer?: Transfer;
}