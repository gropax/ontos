import { Component, OnInit, Input } from '@angular/core';
import { Page, Expression } from '../../../models/graph';
import { ConfigService } from '../../../services/config.service';

@Component({
  selector: 'app-page-title',
  templateUrl: './page-title.component.html',
  styleUrls: ['./page-title.component.css']
})
export class PageTitleComponent implements OnInit {

  @Input() page: Page;
  @Input() prefix: string;

  constructor(
    private configService: ConfigService) {
  }

  ngOnInit() {
  }

  private get referenceCount() {
    return this.page.references.length;
  }

  private get title() {
    let title = this.mainLabel;
    if (this.prefix)
      title = `${this.prefix} « ${title} »`;
    return title;
  }

  private get mainLabel() {
    if (this.referenceCount == 0)
      return "Untitled";
    else if (this.referenceCount == 1)
      return this.formatLabel(this.page.references[0].expression);
    else {
      let expressions = this.page.references.map(r => r.expression);
      let fstLanguage = expressions.filter(e => e.language);
      if (fstLanguage.length > 0)
        return this.formatLabel(fstLanguage[0]);
      else
        return this.formatLabel(expressions[0]);
    }
  }

  private formatLabel(expression: Expression) {
    if (expression.language == this.configService.preferredLanguage)
      return expression.label;
    else
      return `${expression.language}. ${expression.label}`;
  }

}
