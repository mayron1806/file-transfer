import { HttpError } from "@/errors/HttpError";

type PrepareReceiveFilesTransferBody = {
  name?: string;
  message?: string;
  maxSize?: number;
  maxFiles?: number;
  acceptedFiles?: string[];
  expiresAt?: Date;
  password?: string;
};
type PrepareReceiveFilesTransferResponse = {
  transferId: string;
  uploadUrl: string;
};
export const prepareReceiveFilesTransfer = async (accessToken: string, organizationId: number, body: PrepareReceiveFilesTransferBody) => {
  const res = await fetch(`/api/organization/${organizationId}/transfer/receive`, {
    method: "POST",
    headers: {
      "Authorization": `Bearer ${accessToken}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  });
  const data = await res.json();
  if (!res.ok) {
    throw new HttpError(data.error, res.status);
  }
  return data as PrepareReceiveFilesTransferResponse;
}