import { Component, OnInit, ViewChild, EventEmitter, Output, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PageSelectorComponent } from '../page-selector/page-selector.component';
import { NewRelation, DirectedRelationType, NewRelatedPage, NewExpression } from '../../../models/graph';
import { PageContentFormComponent } from '../page-content-form/page-content-form.component';

@Component({
  selector: 'app-new-relation-form',
  templateUrl: './new-relation-form.component.html',
  styleUrls: ['./new-relation-form.component.css']
})
export class NewRelationFormComponent implements OnInit {

  relationForm: FormGroup;
  relationTypes = DirectedRelationType.all();

  @ViewChild(PageSelectorComponent, { static: true }) pageSelector: PageSelectorComponent;

  @Input() pageId: string;
  @Output() pageSelected = new EventEmitter<string>();

  public get type() {
    return this.relationForm.get('type').value;
  }
  public get valid() { return true; /* this.form.valid;*/ }

  public getNewRelation() {
    return NewRelation.create(this.type, this.pageId, this.pageSelector.selected.pageId);
  }

  public getNewRelatedPage() {
    return new NewRelatedPage(this.type,
      new NewExpression(this.pageSelector.languageSelector.selectedLanguage, this.pageSelector.selected),
      null);
  }

  constructor() { }

  ngOnInit() {
    this.relationForm = new FormGroup({
      type: new FormControl(this.relationTypes[0], [ Validators.required, ]),
    });
  }


}
