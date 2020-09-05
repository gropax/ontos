import { Component, OnInit, ViewChildren, QueryList, Input } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Reference } from '../../../models/graph';
import { SortColumn, SortDirection } from '../../../models/pagination';
import { SortableHeader } from '../sortable-header.component';
import { ConfigService } from '../../../services/config.service';
import { GraphService } from '../../../services/graph.service';

@Component({
  selector: 'app-reference-table',
  templateUrl: './reference-table.component.html',
  styleUrls: ['./reference-table.component.css']
})
export class ReferenceTableComponent implements OnInit {

  @Input() references$: Observable<Reference[]>;

  //private references$ = new BehaviorSubject<Reference[]>([]);

  constructor(
    private config: ConfigService,
    private graphService: GraphService) {
  }

  ngOnInit() {
  }

}
