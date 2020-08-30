import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NewPage } from '../../../models/graph';

@Component({
  selector: 'app-new-content-form',
  templateUrl: './new-content-form.component.html',
  styleUrls: ['./new-content-form.component.css']
})
export class NewContentFormComponent implements OnInit {

  contentForm: FormGroup;

  get details() { return this.contentForm.get('details'); }

  public get valid() { return this.contentForm.valid; }
  public get model() {
    return new NewPage(
      this.contentForm.get('details').value
    );
  }

  constructor() { }

  ngOnInit() {
    this.contentForm = new FormGroup({
      details: new FormControl('', [ Validators.required, ]),
    });
  }

}
