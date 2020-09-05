import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LanguageSelectorComponent } from '../language-selector/language-selector.component';
import { NewExpression } from '../../../models/graph';

@Component({
  selector: 'app-expression-form',
  templateUrl: './expression-form.component.html',
  styleUrls: ['./expression-form.component.css']
})
export class ExpressionFormComponent implements OnInit {

  expressionForm: FormGroup;
  @Input() required: boolean = true;
  @ViewChild(LanguageSelectorComponent, { static: true }) languageSelector: LanguageSelectorComponent;

  get name() { return this.expressionForm.get('name'); }
  get language() { return this.languageSelector.selectedLanguage; }

  public get valid() { return this.expressionForm.valid; }
  public get model() {
    if (this.name.value)
      return new NewExpression(this.language, this.name.value);
    else
      return null;
  }

  constructor() { }

  ngOnInit() {
    let nameValidators = [];
    if (this.required)
      nameValidators.push(Validators.required);

    this.expressionForm = new FormGroup({
      name: new FormControl('', nameValidators),
    });
  }

}
