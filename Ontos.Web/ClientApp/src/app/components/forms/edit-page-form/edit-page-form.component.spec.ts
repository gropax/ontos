import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPageFormComponent } from './edit-page-form.component';

describe('EditPageFormComponent', () => {
  let component: EditPageFormComponent;
  let fixture: ComponentFixture<EditPageFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPageFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPageFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
