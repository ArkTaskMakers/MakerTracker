import { MenuItemTypes } from "./menuItemTypes.enum";
import { BaseMenuItem } from "./baseMenuItem";

export class RouteMenuItem extends BaseMenuItem {
  text: string;
  route: string[];

  /** Initializes a new instance of the RouteMenuItem class **/
  public constructor(init?: Partial<RouteMenuItem>) {
    super(MenuItemTypes.Route, init);
  }
}
