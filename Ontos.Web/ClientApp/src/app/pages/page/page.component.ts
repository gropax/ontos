import { Component, OnInit } from '@angular/core';
import { Page } from '../../models/graph';
import { ActivatedRoute, Router } from '@angular/router';
import { GraphService } from '../../services/graph.service';
import { faPenFancy, faPenNib, faEraser } from '@fortawesome/free-solid-svg-icons';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PagePage implements OnInit {

  private modal: NgbModalRef;

  private pageId: string;
  private page: Page;

  private loading: boolean = false;
  private deleting: boolean = false;
  private deleteMessage: string;

  editIcon = faPenNib;
  deleteIcon = faEraser;


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private notificationService: NotificationService,
    private modalService: NgbModal,
    private graphService: GraphService) {
  }

  ngOnInit() {
    this.pageId = this.route.snapshot.paramMap.get("id");
    this.deleteMessage = `Are you sure you want to delete the page with ID ${this.pageId}`;

    this.loading = true;
    this.graphService.getPage(this.pageId)
      .subscribe(page => {
        this.loading = false;
        this.page = page;
      }, error => {
        this.loading = false;
        this.notificationService.notifyError(error);
      });
  }
  
  openModal(content) {
    this.modal = this.modalService.open(content, { ariaLabelledBy: 'modal-confirm-deletion' });
    this.modal.result.then(() => {
      this.deleting = true;
      this.graphService.deletePage(this.pageId)
        .subscribe(() => {
          this.deleting = false;

          // TODO: display INFO notification after navigation
          //this.notificationService.notifyInfo("Page deleted", `The page with ID ${this.pageId} has been successfuly deleted`);

          this.router.navigate(['/pages']);
        }, error => {
          this.deleting = false;
          this.notificationService.notifyError(error);
        });
    });
  }

}
