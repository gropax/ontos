import { Component, OnInit, ViewChild } from '@angular/core';
import { EditPageFormComponent } from '../../components/forms/edit-page-form/edit-page-form.component';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../../services/notification.service';
import { GraphService } from '../../services/graph.service';
import { Page } from '../../models/graph';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-page-edit',
  templateUrl: './page-edit.component.html',
  styleUrls: ['./page-edit.component.css']
})
export class PageEditPage implements OnInit {

  @ViewChild(EditPageFormComponent, { static: false }) form: EditPageFormComponent;
  get valid() { return this.form ? this.form.pageForm.valid : false; }

  saveIcon = faSave;

  private page: Page;
  private saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private graphService: GraphService,
    private router: Router) {
  }

  ngOnInit() {
    this.route.data.subscribe((data: { page: Page }) => this.page = data.page)
  }

  submit() {
    this.saving = true;
    this.graphService.updatePage(this.form.model)
      .subscribe(() => {
        this.saving = false;
        this.router.navigate(['/pages', this.page.id]);
      }, error => {
        this.saving = false;
        this.notificationService.notifyError(error);
      });
  }

}
