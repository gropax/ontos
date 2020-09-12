import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageContentFormComponent } from './page-content-form.component';

describe('PageContentFormComponent', () => {
  let component: PageContentFormComponent;
  let fixture: ComponentFixture<PageContentFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageContentFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageContentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
