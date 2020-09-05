import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewPage, NewExpression } from '../../../models/graph';
import { LanguageSelectorComponent } from '../language-selector/language-selector.component';
import { ExpressionFormComponent } from '../expression-form/expression-form.component';

@Component({
  selector: 'app-new-page-form',
  templateUrl: './new-page-form.component.html',
  styleUrls: ['./new-page-form.component.css']
})
export class NewPageFormComponent implements OnInit {

  pageForm: FormGroup;
  @ViewChild(ExpressionFormComponent, { static: true }) expressionForm: ExpressionFormComponent;

  get content() { return this.pageForm.get('content'); }

  public get valid() { return this.pageForm.valid && this.expressionForm.valid; }
  public get model() {
    var newPage = new NewPage(this.content.value);
    if (this.expressionForm.model)
      newPage.expression = this.expressionForm.model;
    return newPage;
  }

  constructor() { }

  ngOnInit() {
    this.pageForm = new FormGroup({
      content: new FormControl('', [ Validators.required, ]),
    });
  }

}
