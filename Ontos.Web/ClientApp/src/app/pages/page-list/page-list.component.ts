import { Component, OnInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { ConfigService } from '../../services/config.service';
import { GraphService } from '../../services/graph.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { Page } from '../../models/graph';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.css']
})
export class PageListPage implements OnInit {

  private modal: NgbModalRef;

  faPlus = faPlus;

  constructor(
    private configService: ConfigService,
    private modalService: NgbModal,
    private router: Router) {
  }

  ngOnInit() {
  }
  
  openModal(content) {
    this.modal = this.modalService.open(content, { ariaLabelledBy: 'modal-new-dataset' });
    this.modal.result.then((page: Page) => {
      this.router.navigate(['/pages', page.id]);
    }, error => {
    });
  }

}
