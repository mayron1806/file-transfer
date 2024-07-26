import { formatBytes } from "@/lib/format-bytes";
import { SimpleTransfer } from "@/types/api/transfer";
import { ColumnDef } from "@tanstack/react-table";
import moment from "moment";
import ColumnActions from "./actions";
import { Checkbox } from "@/components/ui/checkbox";
import React from "react";
export type SendedFilesData = SimpleTransfer;
export const columns: ColumnDef<SendedFilesData>[] = [
  {
    id: 'select-col',
    header: ({ table }) => (
      <div className="flex items-center">
        <Checkbox
          checked={table.getIsAllPageRowsSelected()}
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label="Select all"
        />
      </div>
    ),
    cell: ({ row }) => (
      <div className="flex items-center">
        <Checkbox
          checked={row.getIsSelected()}
          onCheckedChange={(value) => row.toggleSelected(!!value)}
          aria-label="Select row"
        />
      </div>
    ),
  },
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
    accessorKey: 'receive.hasPassword',
    header: 'Senha',
    cell: (props) => props.getValue() ? '✅' : '❌',
  },
  {
    accessorKey: 'size',
    header: 'Tamanho',
    cell: (props) => props.getValue() ? formatBytes(props.getValue() as number) : '',
  },
  {
    accessorKey: 'filesCount',
    header: 'Qtd. de arquivos',
    cell: (props) => props.getValue()
  },
  {
    id: 'actions',
    cell: (props) => <ColumnActions />
  }
]