import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PageSelectorComponent } from '../page-selector/page-selector.component';

@Component({
  selector: 'app-new-relation-form',
  templateUrl: './new-relation-form.component.html',
  styleUrls: ['./new-relation-form.component.css']
})
export class NewRelationFormComponent implements OnInit {

  relationForm: FormGroup;
  @ViewChild(PageSelectorComponent, { static: true }) pageSelector: PageSelectorComponent;

  public get valid() { return true; /* this.form.valid;*/ }
  //public get valid() { return this.pageForm.valid && this.expressionForm.valid; }
  public get model() {
    return {};
  }

  constructor() { }

  ngOnInit() {
    this.relationForm = new FormGroup({
      type: new FormControl('', [ Validators.required, ]),
    });
  }


}
