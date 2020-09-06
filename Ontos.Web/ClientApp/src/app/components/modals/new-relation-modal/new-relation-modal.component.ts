import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../../../services/notification.service';
import { GraphService } from '../../../services/graph.service';
import { RelatedPage } from '../../../models/graph';
import { NewRelationFormComponent } from '../../forms/new-relation-form/new-relation-form.component';

@Component({
  selector: 'app-new-relation-modal',
  templateUrl: './new-relation-modal.component.html',
  styleUrls: ['./new-relation-modal.component.css']
})
export class NewRelationModalComponent implements OnInit {

  @Output() success = new EventEmitter<RelatedPage>();
  @Output() close = new EventEmitter();

  @ViewChild(NewRelationFormComponent, { static: true }) form: NewRelationFormComponent;
  get valid() { return this.form.valid; }

  private loading = false;
  faPlus = faPlus;

  constructor(
    private notificationService: NotificationService,
    private graphService: GraphService) { }

  ngOnInit() {
  }

  submit() {
    this.loading = true;
    //this.graphService.createRelation(this.form.model)
    //  .subscribe(dataset => {
    //    this.loading = false;
    //    this.success.emit(dataset);
    //  }, error => {
    //    this.notificationService.notifyError(error);
    //    this.close.emit();
    //  });
  }

}
