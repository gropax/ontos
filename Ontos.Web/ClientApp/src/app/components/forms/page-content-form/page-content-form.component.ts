import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-page-content-form',
  templateUrl: './page-content-form.component.html',
  styleUrls: ['./page-content-form.component.css']
})
export class PageContentFormComponent implements OnInit {

  contentForm: FormGroup;
  @ViewChild('content', { static: true }) contentElement: ElementRef;

  get content() { return this.contentForm.get('content'); }

  public get valid() { return this.contentForm.valid }
  public get model() { return this.content.value; }

  constructor() { }

  ngOnInit() {
    this.contentForm = new FormGroup({
      content: new FormControl('', [ Validators.required, ]),
    });
  }

}
