import { Component, OnInit, Input } from '@angular/core';
import { PageType, UpdatePage } from '../../../models/graph';
import { GraphService } from '../../../services/graph.service';

@Component({
  selector: 'app-page-type-selector',
  templateUrl: './page-type-selector.component.html',
  styleUrls: ['./page-type-selector.component.css']
})
export class PageTypeSelectorComponent implements OnInit {

  @Input() pageId: string;
  @Input() pageType: string;

  private loading = false;
  private types: PageType[];
  private type: PageType;

  constructor(private graphService: GraphService) {
    this.types = PageType.all();
  }

  ngOnInit() {
    this.type = PageType.parse(this.pageType);
  }

  updateType(type: PageType) {
    this.loading = true;
    this.graphService.updatePage(new UpdatePage(this.pageId, null, type.id))
      .subscribe(page => {
        this.loading = false;
        this.type = type;
      }, err => {
        this.loading = false;
      });
  }

}
