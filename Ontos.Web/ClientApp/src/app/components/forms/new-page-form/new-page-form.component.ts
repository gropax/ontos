import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewPage, NewExpression } from '../../../models/graph';
import { LanguageSelectorComponent } from '../language-selector/language-selector.component';

@Component({
  selector: 'app-new-page-form',
  templateUrl: './new-page-form.component.html',
  styleUrls: ['./new-page-form.component.css']
})
export class NewPageFormComponent implements OnInit {

  pageForm: FormGroup;
  @ViewChild(LanguageSelectorComponent, { static: true }) languageSelector: LanguageSelectorComponent;

  get name() { return this.pageForm.get('name'); }
  get language() { return this.languageSelector.selectedLanguage; }
  get content() { return this.pageForm.get('content'); }

  public get valid() { return this.pageForm.valid; }
  public get model() {
    var newPage = new NewPage(this.content.value);
    if (this.name.value)
      newPage.expression = new NewExpression(this.language, this.name.value);
    return newPage;
  }

  constructor() { }

  ngOnInit() {
    this.pageForm = new FormGroup({
      name: new FormControl('', [ ]),
      content: new FormControl('', [ Validators.required, ]),
    });
  }

}
