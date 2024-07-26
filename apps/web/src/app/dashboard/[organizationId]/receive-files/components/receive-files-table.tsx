'use client';

import { TransferDataTable } from "@/components/transfer-table-data";
import { useOrganization } from "@/context/organization-context";
import { useAuth } from "@/hooks/use-auth";
import { columns } from "./columns";
import { getTransfers } from "@/usecases/get-transfers";
import { usePagination } from "@/hooks/use-pagination";
import { TableDataLoader } from "@/components/transfer-table-data/types";
import { SimpleTransfer } from "@/types/api/transfer";

const TABLE_TRANSFER_TYPE = "receive";
const ReceiveFilesTable = () => {
  const { organization } = useOrganization();
  const { accessToken } = useAuth();
  const { pagination, setPagination } = usePagination();
  const dataLoader = async () => {
    const response = await getTransfers(organization.id, accessToken, TABLE_TRANSFER_TYPE, pagination);
    return { rowsCount: response.transfersCount, rows: response.transfers } as TableDataLoader<SimpleTransfer>;
  }
  return (
    <TransferDataTable 
      columns={columns}
      dataLoader={dataLoader}
      pagination={pagination}
      type={TABLE_TRANSFER_TYPE}
      setPagination={setPagination}
    />
  );
}
 
export default ReceiveFilesTable;