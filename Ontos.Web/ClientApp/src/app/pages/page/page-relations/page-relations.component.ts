import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { GraphService } from '../../../services/graph.service';
import { BehaviorSubject } from 'rxjs';
import { Relation, RelatedPage } from '../../../models/graph';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-page-relations',
  templateUrl: './page-relations.component.html',
  styleUrls: ['./page-relations.component.css']
})
export class PageRelationsComponent implements OnInit {

  @Input() pageId: string;

  private relatedPages$ = new BehaviorSubject<RelatedPage[]>([]);

  constructor(private graphService: GraphService) {
  }

  ngOnInit() {
    this.loadRelatedPages();
  }

  //ngOnChanges(changes: SimpleChanges) {
  //  this.loadRelatedPages();
  //}

  loadRelatedPages() {
    this.graphService.getRelatedPages(this.pageId)
      .subscribe(relatedPages => {
        this.relatedPages$.next(relatedPages);
      });
  }

}
