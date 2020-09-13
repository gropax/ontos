import { Injectable } from '@angular/core';
import { Page, Expression } from '../models/graph';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class PageTitleService {

  constructor(private configService: ConfigService) { }

  public all(page: Page) {
  }

  public getMainTitle(page: Page, prefix: string = null) {
    let title = this.mainLabel(page);
    if (prefix)
      title = `${prefix} « ${title} »`;
    return title;
  }

  private mainLabel(page: Page) {
    var refCount = page.references.length;

    if (refCount == 0)
      return "Untitled";
    else if (refCount == 1)
      return this.formatLabel(page.references[0].expression);
    else {
      let expressions = page.references.map(r => r.expression);
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
