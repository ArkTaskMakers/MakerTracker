import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';

/** Acts as a landing page for the application. */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  /**
   * Initializes a new instance of the HomeComponent class.
   * @param auth The authentication service, for displaying the register/go to dashboard button on the landing.
   */
  constructor(public auth: AuthService) {
  }
}
