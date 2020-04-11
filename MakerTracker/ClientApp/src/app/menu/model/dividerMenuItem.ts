import { BaseMenuItem } from "./baseMenuItem";
import { MenuItemTypes } from "./menuItemTypes.enum";

export class DividerMenuItem extends BaseMenuItem {

  /** Initializes a new instance of the DividerMenuItem class **/
  public constructor(init?: Partial<DividerMenuItem>) {
    super(MenuItemTypes.Divider, init);
  }
}
