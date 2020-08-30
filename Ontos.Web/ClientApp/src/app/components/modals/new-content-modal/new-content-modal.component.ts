import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { GraphService } from '../../../services/graph.service';
import { NewContentFormComponent } from '../../forms/new-content-form/new-content-form.component';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../../../services/notification.service';
import { Page } from '../../../models/graph';

@Component({
  selector: 'app-new-content-modal',
  templateUrl: './new-content-modal.component.html',
  styleUrls: ['./new-content-modal.component.css']
})
export class NewContentModalComponent implements OnInit {

  @Output() success = new EventEmitter<Page>();
  @Output() close = new EventEmitter();

  @ViewChild(NewContentFormComponent, { static: true }) form: NewContentFormComponent;
  get valid() { return this.form.contentForm.valid; }

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
