import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
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

  /** Notifies the UI of layout changes */
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(1)
  );

  /**
   * Initializes a new instance of the FooterNavComponent class.
   * @param breakpointObserver observes for reactive breakpoints to notify of layout changes
   */
  constructor(private breakpointObserver: BreakpointObserver) {}
}
