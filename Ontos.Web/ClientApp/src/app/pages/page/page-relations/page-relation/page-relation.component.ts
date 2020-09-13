import { Component, OnInit, Input } from '@angular/core';
import { Relation, RelatedPage } from '../../../../models/graph';
import { PageTitleService } from '../../../../services/page-title.service';

@Component({
  selector: 'app-page-relation',
  templateUrl: './page-relation.component.html',
  //host: {'class': 'card'},
  styleUrls: ['./page-relation.component.css']
})
export class PageRelationComponent implements OnInit {

  @Input() relatedPage: RelatedPage;

  constructor(
    private pageTitleService: PageTitleService) {
  }

  ngOnInit() {
  }

  get title() {
    return this.pageTitleService.getMainTitle(this.relatedPage.target);
  }

}
