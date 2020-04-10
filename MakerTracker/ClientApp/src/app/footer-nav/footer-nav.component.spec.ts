import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterNavComponent } from './footer-nav.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { of, BehaviorSubject } from 'rxjs';
import { BreakpointObserver } from '@angular/cdk/layout';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';

class MockBreakpointObserver {
  subject = new BehaviorSubject<any>({ matches: true });
  observe = jasmine.createSpy().and.returnValue(this.subject);
}

describe('FooterNavComponent', () => {
  let component: FooterNavComponent;
  let fixture: ComponentFixture<FooterNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FooterNavComponent ],
      imports: [ MatButtonModule, MatToolbarModule, MatMenuModule, MatIconModule],
      providers: [{ provide: BreakpointObserver, useClass: MockBreakpointObserver }]
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

  it('should get handset layout', (done) => {
    component.isHandset$.subscribe(res => {
      expect(res).toBe(true);
      done();
    })
  });

  it('should get normal layout', (done) => {
    const obs: MockBreakpointObserver = TestBed.get(BreakpointObserver);
    obs.subject.next({ matches: false });
    component.isHandset$.subscribe(res => {
      expect(res).toBe(false);
      done();
    })
  });
});
