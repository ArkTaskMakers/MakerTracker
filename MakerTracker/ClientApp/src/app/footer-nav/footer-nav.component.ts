import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer-nav',
  templateUrl: './footer-nav.component.html',
  styleUrls: ['./footer-nav.component.css']
})
export class FooterNavComponent implements OnInit {

  copyrightYear: string;

  constructor() {
    const currentYear = new Date().getFullYear();
    this.copyrightYear = currentYear === 2020 ? '2020' : `2020-${currentYear}`;
  }

  ngOnInit(): void {
  }
}
