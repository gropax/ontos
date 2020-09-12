import { Component, OnInit, ViewChild } from '@angular/core';
import { Page, PageSearch, PageSearchResult } from '../../../models/graph';
import { Observable, of } from 'rxjs';
import { debounceTime, map, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { GraphService } from '../../../services/graph.service';
import { LanguageSelectorComponent } from '../language-selector/language-selector.component';

@Component({
  selector: 'app-page-selector',
  templateUrl: './page-selector.component.html',
  styleUrls: ['./page-selector.component.css']
})
export class PageSelectorComponent implements OnInit {

  private searching = false;
  private searchFailed = false;
  private page: Page;

  @ViewChild(LanguageSelectorComponent, { static: true }) languageSelector: LanguageSelectorComponent;

  get language() { return this.languageSelector.selectedLanguage; }

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
            return of<PageSearchResult[]>([]);
          }))
      ),
      tap(() => this.searching = false)
    )

  formatter = (page: Page) => page.references[0].expression.label;

}
