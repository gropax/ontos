import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewRelationFormComponent } from './new-relation-form.component';

describe('NewRelationFormComponent', () => {
  let component: NewRelationFormComponent;
  let fixture: ComponentFixture<NewRelationFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewRelationFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewRelationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
