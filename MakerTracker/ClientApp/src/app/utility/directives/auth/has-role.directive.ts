import { Directive } from '@angular/core';
import { Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { HasRoleContext } from './has-role-context';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
  private requiredRoles: any[] = [];
  constructor(
    private _templateRef: TemplateRef<HasRoleContext>,
    private _viewContainer: ViewContainerRef,
    private _authService: AuthService
  ) {
    _authService.isLoggedIn$.subscribe(() => {
      this.evaluateRoles(this.requiredRoles);
    });
  }

  @Input() set appHasRole(roles: any[]) {
    this.requiredRoles = roles;
    this.evaluateRoles(roles);
  }

  private evaluateRoles(roles: any[]): void {
    this._viewContainer.clear();
    if (roles.some((role) => this._authService.hasRole(role))) {
      this._viewContainer.createEmbeddedView(this._templateRef);
    }
  }
}
