import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestorListComponent } from './requestor-list.component';

describe('RequestorListComponent', () => {
  let component: RequestorListComponent;
  let fixture: ComponentFixture<RequestorListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestorListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
