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
import { MatDialog } from "@angular/material/dialog";
import { InitProfileComponent } from "../Profiles/init-profile/init-profile.component";
import { BackendService } from "../services/backend/backend.service";

@Component({
  selector: "app-menu",
  templateUrl: "./menu.component.html",
  styleUrls: ["./menu.component.scss"],
})
export class MenuComponent implements OnInit, OnDestroy {
  private _subscriptions: Subscription[];
  menuItemTypes = MenuItemTypes;
  menuItems: BaseMenuItem[] = [
    new RouteMenuItem({
      text: "My Profile",
      route: ["/profile"],
      requiresAuth: true,
    }),
    new DividerMenuItem({
      requiresAuth: true,
    }),
    new RouteMenuItem({
      text: "My Dashboard",
      route: ["/dashboard"],
      requiresAuth: true,
    }),
    new DividerMenuItem({
      requiresAuth: true,
      requiresAdmin: true,
    }),
    new DropdownMenuItem({
      text: "Admin",
      requiresAuth: true,
      requiresAdmin: true,
      items: [
        new RouteMenuItem({
          text: "Manage Products",
          route: ["/products"],
        }),
        new RouteMenuItem({
          text: "Manage Equipment",
          route: ["/equipment"],
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
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private backend: BackendService
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
              (!i.requiresAdmin || this.auth.hasRole("Admin")) &&
              (!i.requiresAuth || isLoggedIn) &&
              (!i.requiresAnon || !isLoggedIn)
          )
        );
      }),
      this.auth.userProfile$.subscribe((user) => {
        if (user) {
          this.backend.getProfile().subscribe(
            (profile) => true,
            (error) => {
              if (error.status === 404) {
                const dialogRef = this.dialog.open(InitProfileComponent, {
                  height: "600px",
                  width: "400px",
                  closeOnNavigation: false,
                  disableClose: true
                });
              }
            }
          );
        }
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
