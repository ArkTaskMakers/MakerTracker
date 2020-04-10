import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

/**
 * Displays a nav toolbar at the bottom to global pages along with copyright info.
 */
@Component({
  selector: 'app-footer-nav',
  templateUrl: './footer-nav.component.html',
  styleUrls: ['./footer-nav.component.scss']
})
export class FooterNavComponent {
  public originYear = 2020;

  /** gets the formatted copyright year from origin to present. */
  get copyrightYear(): string {
    const currentYear = new Date().getFullYear();
    return currentYear === this.originYear ? `${this.originYear}` : `${this.originYear}-${currentYear}`;
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay(1)
    );

  constructor(private breakpointObserver: BreakpointObserver) {}
}
