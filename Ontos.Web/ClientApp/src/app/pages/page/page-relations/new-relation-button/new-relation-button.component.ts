import { Component, OnInit, Input } from '@angular/core';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { faShareAlt } from '@fortawesome/free-solid-svg-icons';
import { RelatedPage } from '../../../../models/graph';

@Component({
  selector: 'app-new-relation-button',
  templateUrl: './new-relation-button.component.html',
  styleUrls: ['./new-relation-button.component.css']
})
export class NewRelationButtonComponent implements OnInit {

  @Input() pageId: string;

  private modal: NgbModalRef;

  buttonIcon = faShareAlt;

  constructor(
    private modalService: NgbModal) {
  }

  ngOnInit() {
  }
  
  openModal(content) {
    this.modal = this.modalService.open(content, { ariaLabelledBy: 'modal-new-dataset', centered: true });
    this.modal.result.then((relatedPage: RelatedPage) => {
    }, error => {
    });
  }

}
