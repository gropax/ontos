import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferenceInlineFormComponent } from './reference-inline-form.component';

describe('ReferenceInlineFormComponent', () => {
  let component: ReferenceInlineFormComponent;
  let fixture: ComponentFixture<ReferenceInlineFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferenceInlineFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferenceInlineFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
