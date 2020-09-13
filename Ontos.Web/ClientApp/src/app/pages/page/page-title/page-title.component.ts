import { Component, OnInit, Input } from '@angular/core';
import { Page, Expression } from '../../../models/graph';
import { ConfigService } from '../../../services/config.service';
import { PageTitleService } from '../../../services/page-title.service';

@Component({
  selector: 'app-page-title',
  templateUrl: './page-title.component.html',
  styleUrls: ['./page-title.component.css']
})
export class PageTitleComponent implements OnInit {

  @Input() page: Page;
  @Input() prefix: string;

  constructor(private pageTitleService: PageTitleService) {
  }

  ngOnInit() {
  }

  private get referenceCount() {
    return this.page.references.length;
  }

  private get title() {
    return this.pageTitleService.getMainTitle(this.page, this.prefix);
  }

}
