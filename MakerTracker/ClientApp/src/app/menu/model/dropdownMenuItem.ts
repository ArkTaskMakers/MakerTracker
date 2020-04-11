import { MenuItemTypes } from "./menuItemTypes.enum";
import { BaseMenuItem } from "./baseMenuItem";

export class DropdownMenuItem extends BaseMenuItem {
  text: string;
  items: BaseMenuItem[];

  /** Initializes a new instance of the DropdownMenuItem class **/
  public constructor(init?: Partial<DropdownMenuItem>) {
    super(MenuItemTypes.Dropdown, init);
  }
}
