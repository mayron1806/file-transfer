"use client";

import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  PaginationState,
  RowSelectionState,
  Updater,
  useReactTable,
} from "@tanstack/react-table";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { useQuery, keepPreviousData } from "@tanstack/react-query";
import { useMemo, useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { TableDataLoader } from "./types";
import { Pagination } from "@/types/pagination";
import { Trash2Icon } from "lucide-react";
type T = object & {
  id: string | number;
}
interface TransferDataTableProps<TData extends T, TValue> {
  columns: ColumnDef<TData, TValue>[];
  dataLoader: () => Promise<TableDataLoader<TData>>;
  pagination: Pagination;
  type: string;
  setPagination: (p: Pagination) => void;
}
export function TransferDataTable<TData extends T, TValue>({
  columns,
  dataLoader,
  pagination,
  type,
  setPagination,
}: TransferDataTableProps<TData, TValue>) {
  const [rowSelection, setRowSelection] = useState<RowSelectionState>({}) 
  const paginationState = {
    pageIndex: pagination.page,
    pageSize: pagination.limit,
  }
  const dataQuery = useQuery({
    queryKey: [type, pagination],
    placeholderData: keepPreviousData,
    queryFn: dataLoader,
  });
  const onPaginationChange = (paginationUpdater: Updater<PaginationState>) => {
    if(typeof paginationUpdater === 'function') {
      const newState = paginationUpdater(paginationState);
      setPagination({ limit: newState.pageSize, page: newState.pageIndex });
    } else {
      setPagination({ limit: paginationUpdater.pageSize, page: paginationUpdater.pageIndex });
    }
  }
  const defaultData = useMemo(() => [], []);
  const table = useReactTable<TData>({
    data: dataQuery.data?.rows ?? defaultData,
    columns,
    rowCount: dataQuery.data?.rowsCount,
    state: {
      pagination: paginationState,
      rowSelection,
    },
    onRowSelectionChange: setRowSelection,
    enableRowSelection: true,
    getRowId: (row) => row.id.toString(),
    onPaginationChange,
    getCoreRowModel: getCoreRowModel(),
    manualFiltering: true,
    manualPagination: true,
  });

  return (
    <>
      {
        table.getRowModel().rows.filter(row => row.getIsSelected()).length > 0 && (
          <div className="flex justify-between items-center gap-2 mb-2">
            <p>Selecionados: {table.getRowModel().rows.filter(row => row.getIsSelected()).length} de {table.getPreFilteredRowModel().rows.length}</p>
            <div className="flex gap-2">
              <Button variant="destructive" size="sm">
                <Trash2Icon  className="w-4 h-4 mr-2"/>
                Excluir selecionados
              </Button>
            </div>
          </div>
        )
      }
      <Table>
        <TableHeader>
          {table.getHeaderGroups().map((headerGroup) => (
            <TableRow key={headerGroup.id}>
              {headerGroup.headers.map((header) => {
                return (
                  <TableHead key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </TableHead>
                )
              })}
            </TableRow>
          ))}
        </TableHeader>
        <TableBody>
          {table.getRowModel().rows?.length ? (
            table.getRowModel().rows.map((row) => (
              <TableRow
                key={row.id}
                onClick={row.getToggleSelectedHandler()}
                className={row.getIsSelected() ? 'selected' : ''}
                data-state={row.getIsSelected() && "selected"}
              >
                {row.getVisibleCells().map((cell) => (
                  <TableCell key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </TableCell>
                ))}
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={columns.length} className="h-24 text-center">
                Ainda sem arquivos.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
      <div className="w-full flex justify-between gap-4 items-center mt-2">
        <div>
          {`Página ${table.getState().pagination.pageIndex + 1} de ${table.getPageCount()}`}
        </div>
        <div className="space-x-4">
          <Button
            variant="outline"
            size="default"
            onClick={() => table.firstPage()}
            disabled={!table.getCanPreviousPage()}
          >
            {'<<'}
          </Button>
          <Button
            variant="outline"
            size="default"
            onClick={() => table.previousPage()}
            disabled={!table.getCanPreviousPage()}
          >
            {'<'}
          </Button>
          <Button
            variant="outline"
            size="default"
            onClick={() => table.nextPage()}
            disabled={!table.getCanNextPage()}
          >
            {'>'}
          </Button>
          <Button
            variant="outline"
            size="default"
            onClick={() => table.lastPage()}
            disabled={!table.getCanNextPage()}
          >
            {'>>'}
          </Button>
        </div>
        <Select
          value={table.getState().pagination.pageSize.toString()}
          onValueChange={value => {
            if(isNaN(Number(value))) return;
            table.setPageSize(Number(value));
          }}
        >
          <SelectTrigger className="w-[160px]">
            <SelectValue placeholder="Itens por pagina"/>
          </SelectTrigger>
          <SelectContent>
            <SelectGroup className="text-center">
              <SelectLabel>Quantidade de Itens</SelectLabel>
              <SelectItem value="5">  5 Itens</SelectItem>
              <SelectItem value="10">10 Itens</SelectItem>
              <SelectItem value="20">20 Itens</SelectItem>
              <SelectItem value="30">30 Itens</SelectItem>
              <SelectItem value="40">40 Itens</SelectItem>
              <SelectItem value="50">50 Itens</SelectItem>
            </SelectGroup>
          </SelectContent>
        </Select>
      </div>
    </>
  )
}