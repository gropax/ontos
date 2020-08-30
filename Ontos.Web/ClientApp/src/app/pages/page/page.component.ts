import { Component, OnInit } from '@angular/core';
import { Page } from '../../models/graph';
import { ActivatedRoute } from '@angular/router';
import { GraphService } from '../../services/graph.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PagePage implements OnInit {

  private pageId: string;
  private page: Page;
  private loading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private graphService: GraphService) {
  }

  ngOnInit() {
    this.pageId = this.route.snapshot.paramMap.get("id");

    this.loading = true;
    this.graphService.getPage(this.pageId)
      .subscribe(page => {
        this.page = page;
        this.loading = false;
      });
  }

}
