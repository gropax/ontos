import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteReferenceButtonComponent } from './delete-reference-button.component';

describe('DeleteReferenceButtonComponent', () => {
  let component: DeleteReferenceButtonComponent;
  let fixture: ComponentFixture<DeleteReferenceButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteReferenceButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteReferenceButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
