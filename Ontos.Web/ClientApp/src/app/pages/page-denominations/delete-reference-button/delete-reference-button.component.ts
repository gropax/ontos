import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Reference } from '../../../models/graph';
import { faEraser } from '@fortawesome/free-solid-svg-icons';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GraphService } from '../../../services/graph.service';
import { NotificationService } from '../../../services/notification.service';

@Component({
  selector: 'app-delete-reference-button',
  templateUrl: './delete-reference-button.component.html',
  styleUrls: ['./delete-reference-button.component.css']
})
export class DeleteReferenceButtonComponent implements OnInit {

  @Input() reference: Reference;
  @Output() deleted = new EventEmitter<Reference>();

  private deleting = false;
  private modal: NgbModalRef;

  private get deleteMessage() {
    return `Are you sure you want to delete reference « ${this.reference.expression.label} » ?`;
  }

  deleteRefIcon = faEraser;

  constructor(
    private graphService: GraphService,
    private notificationService: NotificationService,
    private modalService: NgbModal) {
  }

  ngOnInit() {
  }
  
  openModal(content) {
    this.modal = this.modalService.open(content, { ariaLabelledBy: 'modal-confirm-deletion', centered: true });
    this.modal.result.then(() => {
      this.deleting = true;
      this.graphService.deleteReference(this.reference.id)
        .subscribe(() => {
          this.deleting = false;
          this.deleted.emit(this.reference);
        }, error => {
          this.deleting = false;
          this.notificationService.notifyError(error);
        });
    }, () => { /* add this lambda to avoid console error when dismissing modal */ });
  }

}
