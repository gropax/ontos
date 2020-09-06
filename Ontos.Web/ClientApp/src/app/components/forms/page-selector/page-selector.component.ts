import { Component, OnInit } from '@angular/core';
import { Page, PageSearch } from '../../../models/graph';
import { Observable, of } from 'rxjs';
import { debounceTime, map, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { GraphService } from '../../../services/graph.service';

@Component({
  selector: 'app-page-selector',
  templateUrl: './page-selector.component.html',
  styleUrls: ['./page-selector.component.css']
})
export class PageSelectorComponent implements OnInit {

  private searching = false;
  private searchFailed = false;
  private page: Page;
  private language: string;

  constructor(private graphService: GraphService) { }

  ngOnInit() {
  }

  search = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term =>
        this.graphService.searchPages(new PageSearch(this.language, term)).pipe(
          tap(() => this.searchFailed = false),
          catchError(() => {
            this.searchFailed = true;
            return of<Page[]>([]);
          }))
      ),
      tap(() => this.searching = false)
    )

  formatter = (page: Page) => page.references[0].expression.label;

}
