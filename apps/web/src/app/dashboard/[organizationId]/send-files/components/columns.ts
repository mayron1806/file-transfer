import { formatBytes } from "@/lib/format-bytes";
import { SimpleTransfer } from "@/types/api/transfer"
import { ColumnDef } from "@tanstack/react-table"
import moment from "moment";
 
export type SendedFilesData = SimpleTransfer;
export const columns: ColumnDef<SendedFilesData>[] = [
  {
    accessorKey: 'id',
    header: 'ID',
  },
  {
    accessorKey: 'name',
    header: 'Nome / key',
  },
  {
    accessorKey: 'createdAt',
    header: 'Criado em',
    cell: (props) => props.getValue() ? moment(props.getValue() as Date).format('DD/MM/YYYY HH:mm:ss') : '',
  },
  {
    accessorKey: 'expiresAt',
    header: 'Expira em',
    cell: (props) => props.getValue() ? moment(props.getValue() as Date).format('DD/MM/YYYY HH:mm:ss') : '',
  },
  {
    accessorKey: 'send.hasPassword',
    header: 'Senha',
    cell: (props) => props.getValue() ? '✅' : '❌',
  },
  {
    accessorKey: 'send.expiresOnDownload',
    header: 'Expira ao baixar',
    cell: (props) => props.getValue() ? '✅' : '❌',
  },
  {
    accessorFn: (row) => row.send?.downloads,
    header: 'Qtd. de downloads',
  },
  {
    accessorKey: 'size',
    header: 'Tamanho',
    cell: (props) => formatBytes(props.getValue() as number),
  },
  {
    accessorKey: 'filesCount',
    header: 'Qtd. de arquivos',
  }
]