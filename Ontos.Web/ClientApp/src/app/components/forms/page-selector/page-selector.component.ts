import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
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
  public selected: any;

  @ViewChild(LanguageSelectorComponent, { static: true }) languageSelector: LanguageSelectorComponent;

  get language() { return this.languageSelector.selectedLanguage; }
  get isPageSelected() { return this.selected && typeof this.selected != 'string'; }

  @Output() pageSelected = new EventEmitter<string>();

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

  formatter = (input: any) => {
    if (typeof input == 'string')
      return input;
    else {
      this.pageSelected.emit(input.pageGuid);
      return input.expressions.join(", ");
    }
  };

  private unselectPage() {
    if (this.isPageSelected) {
      this.selected = this.selected.expressions.join(", ");
      this.pageSelected.emit(null);
    }
  }

}
