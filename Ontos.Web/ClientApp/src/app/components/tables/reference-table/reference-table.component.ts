import { Component, OnInit, ViewChildren, QueryList, Input, Output, EventEmitter } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Reference } from '../../../models/graph';
import { SortColumn, SortDirection } from '../../../models/pagination';
import { SortableHeader } from '../sortable-header.component';
import { ConfigService } from '../../../services/config.service';
import { GraphService } from '../../../services/graph.service';
import { faEraser } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-reference-table',
  templateUrl: './reference-table.component.html',
  styleUrls: ['./reference-table.component.css']
})
export class ReferenceTableComponent implements OnInit {

  @Input() references$: Observable<Reference[]>;
  @Output() delete = new EventEmitter<Reference>();

  deleteRefIcon = faEraser;

  constructor(
    private config: ConfigService,
    private graphService: GraphService) {
  }

  ngOnInit() {
  }

}
