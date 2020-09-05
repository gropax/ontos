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

  private page: Page;
  private saving: boolean = false;

  private references$ = new BehaviorSubject<Reference[]>([]);

  constructor(
    private route: ActivatedRoute,
    private graphService: GraphService,
    private router: Router) {
  }

  ngOnInit() {
    this.route.data.subscribe((data: { page: Page, references: Reference[] }) => {
      this.page = data.page;
      this.references$.next(data.references);
    });
  }

  loadReferences() {
    this.graphService
      .getReferences(this.page.id)
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
