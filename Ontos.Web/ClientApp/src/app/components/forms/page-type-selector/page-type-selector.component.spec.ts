import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageTypeSelectorComponent } from './page-type-selector.component';

describe('PageTypeSelectorComponent', () => {
  let component: PageTypeSelectorComponent;
  let fixture: ComponentFixture<PageTypeSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageTypeSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageTypeSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
