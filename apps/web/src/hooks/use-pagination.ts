import { Pagination } from "@/types/pagination";
import { useRouter, useSearchParams } from "next/navigation";

export const usePagination = () => {
  const searchParams = useSearchParams();
  const router = useRouter();
  const pagination: Pagination = {
    page: parseInt(searchParams.get('page') ?? '0'),
    limit: parseInt(searchParams.get('limit') ?? '10'),
  };
  const setPagination = (pagination: Pagination) => {
    const params = new URLSearchParams();
    params.append("page", pagination.page.toString());
    params.append("limit", pagination.limit.toString());
    router.push(`?${params.toString()}`);
  }
  const resetPagination = () => {
    router.push('?');
  }
  return {
    setPagination,
    pagination,
    resetPagination,
  }
}