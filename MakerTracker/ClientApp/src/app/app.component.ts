import { Component } from '@angular/core';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent {
  showFeedbackButton = false;

  public constructor(public auth: AuthService) {
    this.auth.isLoggedIn$.subscribe((isLoggedIn) => {
      this.showFeedbackButton = isLoggedIn;
    });
  }
}
