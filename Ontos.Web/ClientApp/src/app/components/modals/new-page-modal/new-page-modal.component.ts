import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { GraphService } from '../../../services/graph.service';
import { NewPageFormComponent } from '../../forms/new-page-form/new-page-form.component';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../../../services/notification.service';
import { Page } from '../../../models/graph';

@Component({
  selector: 'app-new-page-modal',
  templateUrl: './new-page-modal.component.html',
  styleUrls: ['./new-page-modal.component.css']
})
export class NewPageModalComponent implements OnInit {

  @Output() success = new EventEmitter<Page>();
  @Output() close = new EventEmitter();

  @ViewChild(NewPageFormComponent, { static: true }) form: NewPageFormComponent;
  get valid() { return this.form.pageForm.valid; }

  private loading = false;
  faPlus = faPlus;

  constructor(
    private notificationService: NotificationService,
    private graphService: GraphService) { }

  ngOnInit() {
  }

  submit() {
    this.loading = true;
    this.graphService.createPage(this.form.model)
      .subscribe(dataset => {
        this.loading = false;
        this.success.emit(dataset);
      }, error => {
        this.notificationService.notifyError(error);
        this.close.emit();
      });
  }

}
