import { Component, OnInit, Input } from '@angular/core';
import { GraphService } from '../../../services/graph.service';
import { BehaviorSubject } from 'rxjs';
import { Relation } from '../../../models/graph';

@Component({
  selector: 'app-page-relations',
  templateUrl: './page-relations.component.html',
  styleUrls: ['./page-relations.component.css']
})
export class PageRelationsComponent implements OnInit {

  @Input() pageId: string;

  private relations$ = new BehaviorSubject<Relation[]>([]);

  constructor(private graphService: GraphService) { }

  ngOnInit() {
    this.graphService.getRelatedPages(this.pageId)
      .subscribe(relations => {
        this.relations$.next(relations);
      });
  }

}
