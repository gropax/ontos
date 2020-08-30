

export class PaginationParams<T> {
  constructor(
    public page: number,
    public pageSize: number,
    public sort: SortColumn<T>,
    public sortDir: SortDirection) {
  }

  public toStringParams(): { [param: string]: string } {
    return {
      page: this.page.toString(),
      pageSize: this.pageSize.toString(),
      sortColumn: this.sort.toString(),
      sortDirection: this.sortDir.toString(),
    };
  }
}


export class Paginated<T> {
  constructor(
    public page: number,
    public pageSize: number,
    public sort: string,
    public sortDir: string,
    public total: number,
    public items: T[]) {
  }
}


export class Maybe<T> {
  constructor(
    public hasValue: boolean,
    public value: T) {
  }
}


export type SortColumn<T> = keyof T | '';
export type SortDirection = 'asc' | 'desc' | '';


export interface SortEvent<T> {
  column: SortColumn<T>;
  direction: SortDirection;
}


