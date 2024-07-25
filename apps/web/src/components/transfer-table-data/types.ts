import { Pagination } from "@/types/pagination";

export type TableDataLoader<T> = {
  rows: T[];
  rowsCount: number;
}
export type TableDataLoaderFn<TRow> = (filter: Pagination) => Promise<TableDataLoader<TRow>>;