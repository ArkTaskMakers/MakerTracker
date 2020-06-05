import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonColumnComponent } from './button-column.component';

describe('ButtonColumnComponent', () => {
  let component: ButtonColumnComponent;
  let fixture: ComponentFixture<ButtonColumnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ButtonColumnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ButtonColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
