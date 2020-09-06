import { Component, OnInit, Input } from '@angular/core';
import { Relation } from '../../../../models/graph';

@Component({
  selector: 'app-page-relation',
  templateUrl: './page-relation.component.html',
  styleUrls: ['./page-relation.component.css']
})
export class PageRelationComponent implements OnInit {

  @Input() relation: Relation;

  constructor() { }

  ngOnInit() {
  }

}
