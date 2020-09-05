import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ExpressionFormComponent } from '../expression-form/expression-form.component';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NewReference } from '../../../models/graph';
import { GraphService } from '../../../services/graph.service';

@Component({
  selector: 'app-reference-inline-form',
  templateUrl: './reference-inline-form.component.html',
  styleUrls: ['./reference-inline-form.component.css']
})
export class ReferenceInlineFormComponent implements OnInit {

  @Input() pageId: number;
  @Output() created = new EventEmitter<NewReference>();

  loading = false;
  referenceForm: FormGroup;
  @ViewChild(ExpressionFormComponent, { static: true }) expressionForm: ExpressionFormComponent;

  createIcon = faPlus;

  public get valid() { return this.referenceForm.valid && this.expressionForm.valid; }
  public get model() {
    var newExpression = this.expressionForm.model;
    var newReference = new NewReference(this.pageId, newExpression);
    return newReference;
  }

  constructor(private graphService: GraphService) { }

  ngOnInit() {
    this.referenceForm = new FormGroup({ });
  }

  createReference() {
    this.loading = true;
    this.graphService.createReference(this.model)
      .subscribe(reference => {
        this.loading = false;
        this.created.emit(reference);
        this.expressionForm.name.setValue('');
      });
  }

}
