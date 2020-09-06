import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewRelationButtonComponent } from './new-relation-button.component';

describe('NewRelationButtonComponent', () => {
  let component: NewRelationButtonComponent;
  let fixture: ComponentFixture<NewRelationButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewRelationButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewRelationButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
