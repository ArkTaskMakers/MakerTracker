import { Component, OnInit, OnDestroy } from "@angular/core";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { Observable, BehaviorSubject, Subscription } from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { AuthService } from "../services/auth/auth.service";
import {
  RouteMenuItem,
  ActionMenuItem,
  DropdownMenuItem,
  BaseMenuItem,
  MenuItemTypes,
  DividerMenuItem,
} from "./model";

@Component({
  selector: "app-menu",
  templateUrl: "./menu.component.html",
  styleUrls: ["./menu.component.css"],
})
export class MenuComponent implements OnInit, OnDestroy {
  private _subscriptions: Subscription[];
  menuItemTypes = MenuItemTypes;
  menuItems: BaseMenuItem[] = [
    new RouteMenuItem({
      text: "My Dashboard",
      route: ["/dashboard"],
      requiresAuth: true,
    }),
    new DividerMenuItem({
      requiresAuth: true,
      // TODO: requiresAdmin: true
    }),
    new DropdownMenuItem({
      text: "Admin",
      requiresAuth: true,
      // TODO: requiresAdmin: true
      items: [
        new RouteMenuItem({
          text: "Manage Products",
          route: ["/products"],
        }),
      ],
    }),
    new DividerMenuItem({
      requiresAuth: true,
    }),
    new ActionMenuItem({
      text: "Log In",
      action: () => this.auth.login(),
      requiresAnon: true,
    }),
    new ActionMenuItem({
      text: "Log Out",
      action: () => this.auth.logout(),
      requiresAuth: true,
    }),
  ];

  filteredMenuItems: BehaviorSubject<BaseMenuItem[]> = new BehaviorSubject<
    BaseMenuItem[]
  >(this.menuItems.filter((i) => !i.requiresAuth));

  isCollapsed: boolean = false;

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay(1)
    );

  constructor(
    public auth: AuthService,
    private breakpointObserver: BreakpointObserver
  ) {}

  ngOnInit() {
    this._subscriptions = [
      this.isHandset$.subscribe(
        (isCollapsed) => (this.isCollapsed = isCollapsed)
      ),
      this.auth.isLoggedIn$.subscribe((isLoggedIn) => {
        this.filteredMenuItems.next(
          this.menuItems.filter(
            (i) =>
              (!i.requiresAuth && !i.requiresAnon) ||
              (i.requiresAuth && isLoggedIn) ||
              (i.requiresAnon && !isLoggedIn)
          )
        );
      }),
    ];
  }

  ngOnDestroy() {
    if (this._subscriptions && this._subscriptions.length) {
      this._subscriptions.forEach((sub) => sub.unsubscribe());
    }
    this._subscriptions = null;
  }
}
