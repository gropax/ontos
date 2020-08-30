import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewPage } from '../../../models/graph';

@Component({
  selector: 'app-new-page-form',
  templateUrl: './new-page-form.component.html',
  styleUrls: ['./new-page-form.component.css']
})
export class NewPageFormComponent implements OnInit {

  pageForm: FormGroup;

  get content() { return this.pageForm.get('content'); }

  public get valid() { return this.pageForm.valid; }
  public get model() {
    return new NewPage(
      this.pageForm.get('content').value
    );
  }

  constructor() { }

  ngOnInit() {
    this.pageForm = new FormGroup({
      content: new FormControl('', [ Validators.required, ]),
    });
  }

}
