import { Component } from '@angular/core';

/**
 * Displays a nav toolbar at the bottom to global pages along with copyright info.
 */
@Component({
  selector: 'app-footer-nav',
  templateUrl: './footer-nav.component.html',
  styleUrls: ['./footer-nav.component.css']
})
export class FooterNavComponent {
  public originYear = 2020;

  /** gets the formatted copyright year from origin to present. */
  get copyrightYear(): string {
    const currentYear = new Date().getFullYear();
    return currentYear === this.originYear ? `${this.originYear}` : `${this.originYear}-${currentYear}`;
  }
}
