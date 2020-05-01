import { MenuItemTypes } from './menuItemTypes.enum';

export class BaseMenuItem {
  requiresAuth?: boolean;
  requiresAdmin?: boolean;
  requiresAnon?: boolean;

  /** Initializes a new instance of the BaseMenuItem class **/
  public constructor(public type: MenuItemTypes, init?: Partial<BaseMenuItem>) {
    Object.assign(this, init);
  }
}
