export interface ApiResponseBasic {
    success: boolean;
    message?: string;
    data : any | null;
  }
  export interface ApiResponse<T> {
    success: boolean;
    message?: string;
    data?: T;
  }
  export interface DateRange {
    from?: Date | string; // có thể là Date hoặc ISO string
    to?: Date | string;
  }
  
  export interface SearchRequest<T> {
    globalSearch?: string;
    columnFilters?: { [key: string]: string };
    dateFilters?: { [key: string]: DateRange };
    sortBy: string;
    sortOrder: 'asc' | 'desc';
    page: number;
    pageSize: number;
  }
  export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
  }
  export interface BaseEntity {
    id: number;
    createdAt?: string;    // ISO date string
    updateAt?: string;     // ISO date string
    createBy?: string;
    updateBy?: string;
    status: number;
  }
  export interface IActionDto {
    id: number;
    actionType: string;  // A, E, D
  }