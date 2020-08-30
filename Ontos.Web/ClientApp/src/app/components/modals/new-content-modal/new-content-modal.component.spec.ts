import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewContentModalComponent } from './new-content-modal.component';

describe('NewContentModalComponent', () => {
  let component: NewContentModalComponent;
  let fixture: ComponentFixture<NewContentModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewContentModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewContentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
