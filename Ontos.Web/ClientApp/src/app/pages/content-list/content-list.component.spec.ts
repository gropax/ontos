import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentListPage } from './content-list.component';

describe('ContentListPage', () => {
  let component: ContentListPage;
  let fixture: ComponentFixture<ContentListPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContentListPage ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContentListPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
