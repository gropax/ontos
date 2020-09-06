import { Component, OnInit } from '@angular/core';
import { Page } from '../../models/graph';
import { ActivatedRoute, Router } from '@angular/router';
import { GraphService } from '../../services/graph.service';
import { faPenFancy, faPenNib, faEraser, faAt, faShareAlt } from '@fortawesome/free-solid-svg-icons';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PagePage implements OnInit {

  private modal: NgbModalRef;

  private page: Page;

  private deleting: boolean = false;

  editIcon = faPenNib;
  deleteIcon = faEraser;
  editDenominationsIcon = faAt;
  nodeIcon = faShareAlt;

  isCollapsed = true;

  private get deleteMessage() {
    return `Are you sure you want to delete the page with ID ${this.page.id}`;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private notificationService: NotificationService,
    private modalService: NgbModal,
    private graphService: GraphService) {
  }

  ngOnInit() {
    this.route.data.subscribe((data: { page: Page }) => this.page = data.page)
  }
  
  openModal(content) {
    this.modal = this.modalService.open(content, { ariaLabelledBy: 'modal-confirm-deletion', centered: true });
    this.modal.result.then(() => {
      this.deleting = true;
      this.graphService.deletePage(this.page.id)
        .subscribe(() => {
          this.deleting = false;

          // TODO: display INFO notification after navigation
          //this.notificationService.notifyInfo("Page deleted", `The page with ID ${this.pageId} has been successfuly deleted`);

          this.router.navigate(['/pages']);
        }, error => {
          this.deleting = false;
          this.notificationService.notifyError(error);
        });
    }, () => { /* add this lambda to avoid console error when dismissing modal */ });
  }

}
