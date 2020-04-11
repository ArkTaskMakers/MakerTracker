import { Component, OnInit, OnDestroy } from "@angular/core";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import {
  Observable,
  BehaviorSubject,
  Subject,
  Subscription,
  ReplaySubject,
} from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { AuthService } from "../services/auth/auth.service";

@Component({
  selector: "app-menu",
  templateUrl: "./menu.component.html",
  styleUrls: ["./menu.component.css"],
})
export class MenuComponent implements OnInit, OnDestroy {
  private _subscriptions: Subscription[];

  menuItems: (IActionMenuItem | IRouteMenuItem | IDividerMenuItem | IDropdownMenuItem)[] = [
    {
      text: "My Dashboard",
      route: ["/dashboard"],
      requiresAuth: true,
    },
    {
      isDivider: true,
      requiresAuth: true,
      // TODO: requiresAdmin: true
    },
    {
      text: "Manage Products",
      route: ["/products"],
      requiresAuth: true,
      // TODO: requiresAdmin: true,
    },
    {
      requiresAuth: true,
      isDivider: true,
    },
    {
      text: "Log In",
      action: () => this.auth.login(),
      requiresAnon: true,
    },
    {
      text: "Log Out",
      action: () => this.auth.logout(),
      requiresAuth: true,
    },
  ];

  filteredMenuItems: BehaviorSubject<
    (IActionMenuItem | IRouteMenuItem | IDividerMenuItem | IDropdownMenuItem)[]
  > = new BehaviorSubject<
    (IActionMenuItem | IRouteMenuItem | IDividerMenuItem | IDropdownMenuItem)[]
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
  ) {
  }

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

interface IActionMenuItem extends IBaseMenuItem {
  text: string;
  action: Function;
}

interface IDropdownMenuItem extends IBaseMenuItem {
  text: string;
  items: (IActionMenuItem | IRouteMenuItem | IDividerMenuItem)[];
}

interface IRouteMenuItem extends IBaseMenuItem {
  text: string;
  route: string[];
}

interface IDividerMenuItem extends IBaseMenuItem {
  isDivider: boolean;
}

interface IBaseMenuItem {
  requiresAuth?: boolean;
  requiresAdmin?: boolean;
  requiresAnon?: boolean;
}
