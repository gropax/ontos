import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SortColumn, SortDirection, PaginationParams, SortEvent } from '../../../models/pagination';
import { SortableHeader } from '../sortable-header.component';
import { GraphService } from '../../../services/graph.service';
import { ConfigService } from '../../../services/config.service';
import { Page } from '../../../models/graph';

@Component({
  selector: 'app-page-table',
  templateUrl: './page-table.component.html',
  styleUrls: ['./page-table.component.css']
})
export class PageTableComponent implements OnInit {

  private loading = false;
  private total$ = new BehaviorSubject<number>(0);
  private pages$ = new BehaviorSubject<Page[]>([]);

  private page: number = 1;
  private pageSize: number;
  private sortColumn: SortColumn<Page> = 'id';
  private sortDirection: SortDirection = '';

  private startIndex: number = 0;

  @ViewChildren(SortableHeader) headers: QueryList<SortableHeader<Page>>;

  constructor(
    private config: ConfigService,
    private graphService: GraphService) {
    this.pageSize = config.defaultPageSize;
  }

  ngOnInit() {
    this.loadContent();
  }

  loadContent() {
    this.loading = true;
    this.graphService
      .getContentList(new PaginationParams(this.page, this.pageSize, this.sortColumn, this.sortDirection))
      .subscribe(page => {
        this.loading = false;
        this.pages$.next(page.items);
        this.total$.next(page.total);
      });
  }

  onSort(event: SortEvent<Page>) {
    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== event.column)
        header.direction = '';
    });

    this.sortColumn = event.column;
    this.sortDirection = event.direction;
    this.page = 1;

    this.loadContent();
  }

  onPageChanged(page: number) {
    this.page = page;
    this.startIndex = (this.page - 1) * this.pageSize;
    this.loadContent();
  }

  onPageSizeChanged(pageSize: number) {
    this.page = 1;
    this.pageSize = pageSize;
    this.startIndex = 0;
    this.loadContent();
  }

}
