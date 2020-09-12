import { Component, OnInit, Output, EventEmitter, ViewChild, Input, AfterViewInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../../../services/notification.service';
import { GraphService } from '../../../services/graph.service';
import { RelatedPage, NewRelatedPage } from '../../../models/graph';
import { NewRelationFormComponent } from '../../forms/new-relation-form/new-relation-form.component';
import { PageContentFormComponent } from '../../forms/page-content-form/page-content-form.component';

@Component({
  selector: 'app-new-relation-modal',
  templateUrl: './new-relation-modal.component.html',
  styleUrls: ['./new-relation-modal.component.css']
})
export class NewRelationModalComponent implements OnInit {

  @Input() pageId: string;
  @Output() success = new EventEmitter<RelatedPage>();
  @Output() close = new EventEmitter();

  @ViewChild(NewRelationFormComponent, { static: true }) form: NewRelationFormComponent;
  @ViewChild(PageContentFormComponent, { static: false }) pageContentForm: PageContentFormComponent;

  get valid() { return this.form.valid; }

  private _newPage = false;
  get newPage() { return this._newPage; }
  set newPage(val: boolean) {
    this._newPage = val;
    
    if (val) {  // @fixme Ugly hack to set focus on input field after rendering with *ngIf
      var that = this;
      setTimeout(function () { that.setFocus(); }, 1);
    }
  }

  private loading = false;
  faPlus = faPlus;

  constructor(
    private notificationService: NotificationService,
    private graphService: GraphService) { }

  ngOnInit() {
  }

  setFocus() {
    this.pageContentForm.contentElement.nativeElement.focus();
  }

  createRelation() {
    this.loading = true;
    this.graphService.createRelation(this.form.getNewRelation())
      .subscribe(relatedPage => {
        this.loading = false;
        //this.success.emit(relatedPage);
      }, error => {
        this.notificationService.notifyError(error);
        this.close.emit();
      });
  }

  createRelatedPage() {
    var newRelatedPage = this.form.getNewRelatedPage();
    newRelatedPage.content = this.pageContentForm.model;

    this.loading = true;
    this.graphService.createRelatedPage(this.pageId, newRelatedPage)
      .subscribe(relatedPage => {
        this.loading = false;
        this.success.emit(relatedPage);
      }, error => {
        this.notificationService.notifyError(error);
        this.close.emit();
      });
  }

  onPageSelected() {
    this.newPage = false;
  }

}
