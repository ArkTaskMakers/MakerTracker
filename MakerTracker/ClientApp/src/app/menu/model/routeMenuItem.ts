import { BaseMenuItem } from './baseMenuItem';
import { MenuItemTypes } from './menuItemTypes.enum';

export class RouteMenuItem extends BaseMenuItem {
  text: string;
  route: string[];

  /** Initializes a new instance of the RouteMenuItem class **/
  public constructor(init?: Partial<RouteMenuItem>) {
    super(MenuItemTypes.Route, init);
  }
}
