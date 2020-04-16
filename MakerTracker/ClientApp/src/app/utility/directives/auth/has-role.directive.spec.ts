import { HasRoleDirective } from './has-role.directive';
import { Component } from '@angular/core';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  template: `<div *appHasRole="[Admin]"><h1>test</h1></div>`
})
class TestComponent { }

class AuthServiceSpy {
  shouldHaveRole = true;
  hasRole = jasmine.createSpy('hasRole').and.callFake(() => this.shouldHaveRole);
  isLoggedIn$ = new BehaviorSubject<boolean>(this.shouldHaveRole);
}

describe('HasRoleDirective', () => {
  let fixture: ComponentFixture<TestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HasRoleDirective, TestComponent],
      providers: [{
        provide: AuthService,
        useClass: AuthServiceSpy
      }]
    }).compileComponents();
    fixture = TestBed.createComponent(TestComponent);
    fixture.detectChanges();
  });

  it('should show when roles are true', () => {
    const svc = TestBed.get(AuthService);
    svc.isLoggedIn$.next(false);
    fixture.detectChanges();
    const de = fixture.debugElement.queryAll(By.css('h1'));
    expect(de.length).toBe(1);
  });

  it('should hide when roles are false', () => {
    const svc = TestBed.get(AuthService);
    svc.shouldHaveRole = false;
    svc.isLoggedIn$.next(false);
    fixture.detectChanges();
    const de = fixture.debugElement.queryAll(By.css('h1'));
    expect(de.length).toBe(0);
  });
});
