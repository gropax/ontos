import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewPage, Page, UpdatePage } from '../../../models/graph';

@Component({
  selector: 'app-edit-page-form',
  templateUrl: './edit-page-form.component.html',
  styleUrls: ['./edit-page-form.component.css']
})
export class EditPageFormComponent implements OnInit {

  @Input() page: Page;

  pageForm: FormGroup;

  get content() { return this.pageForm.get('content'); }

  public get valid() { return this.pageForm.valid; }
  public get model() {
    return new UpdatePage(
      this.page.id,
      this.pageForm.get('content').value,
      null
    );
  }

  constructor() { }

  ngOnInit() {
    this.pageForm = new FormGroup({
      content: new FormControl('', [ Validators.required, ]),
    });
  }

}
