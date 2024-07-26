import { HttpError } from "@/errors/HttpError";

type PrepareSendFilesTransferBody = {
  name?: string;
  message?: string;
  files: Array<{ name: string; contentType: string; size: number }>;
  expiresAt?: Date;
  password?: string;
  emailsDestination?: string[];
  expiresOnDownload?: boolean;
};
type PrepareSendFilesTransferResponse = {
  urls: string[];
  transferId: number;
}
export const prepareSendFilesTransfer = async (accessToken: string, organizationId: number, body: PrepareSendFilesTransferBody) => {
  const res = await fetch(`/api/organization/${organizationId}/transfer/send`, {
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
  return data as PrepareSendFilesTransferResponse;
}