import { HttpError } from "@/errors/HttpError";
import { SimpleTransfer } from "@/types/api/transfer";
import { Pagination } from "@/types/pagination";
export type GetTransferResponse = {
  transfers: SimpleTransfer[];
  transfersCount: number;
}

export const getTransfers = async (organizationId: number, accessToken: string, type: "send" | "receive", pagination: Pagination): Promise<GetTransferResponse> => {
  const params = new URLSearchParams();
  params.set("page", pagination.page.toString());
  params.set("limit", pagination.limit.toString());
  params.set("type", type);

  const res = await fetch(`/api/organization/${organizationId}/transfer?${params.toString()}`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    }
  });

  const data = await res.json();
  if (!res.ok) throw new HttpError(data.error, res.status);
  return data as GetTransferResponse;
}