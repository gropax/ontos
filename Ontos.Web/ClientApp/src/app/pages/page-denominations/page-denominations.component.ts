import { Component, OnInit } from '@angular/core';
import { Page, Reference, NewReference } from '../../models/graph';
import { GraphService } from '../../services/graph.service';
import { NotificationService } from '../../services/notification.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-page-denominations',
  templateUrl: './page-denominations.component.html',
  styleUrls: ['./page-denominations.component.css']
})
export class PageDenominationsPage implements OnInit {

  private pageId: number;
  private page: Page;
  private loading: boolean = false;
  private saving: boolean = false;

  private references$ = new BehaviorSubject<Reference[]>([]);

  constructor(
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private graphService: GraphService,
    private router: Router) {
  }

  ngOnInit() {
    this.pageId = parseInt(this.route.snapshot.paramMap.get("id"));

    this.loading = true;
    this.graphService.getPage(this.pageId)
      .subscribe(page => {
        this.page = page;
        this.loading = false;
      });

    this.loadReferences();
  }

  loadReferences() {
    this.graphService
      .getReferences(this.pageId)
      .subscribe(refs => {
        this.references$.next(refs);
      });
  }

  onCreated($event: Reference) {
    this.loadReferences();
  }

  onDeleted($event: Reference) {
    this.loadReferences();
  }

}
