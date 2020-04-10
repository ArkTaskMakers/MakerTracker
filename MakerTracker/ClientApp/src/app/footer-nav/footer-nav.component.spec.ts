import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterNavComponent } from './footer-nav.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';

describe('FooterNavComponent', () => {
  let component: FooterNavComponent;
  let fixture: ComponentFixture<FooterNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FooterNavComponent ],
      imports: [ MatButtonModule, MatToolbarModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should get origin copyright year', () => {
    expect(component).toBeTruthy();
    const currentYear = new Date().getFullYear();
    component.originYear = currentYear;
    expect(component.copyrightYear).toBe(`${currentYear}`);
  });

  it('should get future copyright year', () => {
    expect(component).toBeTruthy();
    const currentYear = new Date().getFullYear();
    component.originYear = 2015;
    expect(component.copyrightYear).toBe(`2015-${currentYear}`);
  });
});
