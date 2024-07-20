import { HttpError } from "@/errors/HttpError";

type ConfirmFileTransferResponse = {
  filesWithError: Array<{ fileId: number, originalName: string, error: string }>;
} 
export const confirmFileTransfer = async (accessToken: string,  organizationId: number, TransferId: number) => {
  const res = await fetch(`/api/organization/${organizationId}/transfer/confirm-receive`, {
    method: "POST",
    headers: {
      "Authorization": `Bearer ${accessToken}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      transferId: TransferId
    }),
  });
  const data = await res.json();
  if (!res.ok) {
    throw new HttpError(data.error, res.status);
  }
  return data as ConfirmFileTransferResponse;
}