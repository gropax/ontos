import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewContentFormComponent } from './new-content-form.component';

describe('NewContentFormComponent', () => {
  let component: NewContentFormComponent;
  let fixture: ComponentFixture<NewContentFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewContentFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewContentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
