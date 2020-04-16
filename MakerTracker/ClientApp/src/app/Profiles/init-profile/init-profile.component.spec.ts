import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InitProfileComponent } from './init-profile.component';

describe('InitProfileComponent', () => {
  let component: InitProfileComponent;
  let fixture: ComponentFixture<InitProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InitProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InitProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
