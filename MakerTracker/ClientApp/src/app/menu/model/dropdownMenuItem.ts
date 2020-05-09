import { BaseMenuItem } from './baseMenuItem';
import { MenuItemTypes } from './menuItemTypes.enum';

export class DropdownMenuItem extends BaseMenuItem {
  text: string | (() => string);
  icon: string;
  items: BaseMenuItem[];

  getText() {
    if (typeof this.text === 'string') {
      return this.text;
    }
    return this.text();
  }

  /** Initializes a new instance of the DropdownMenuItem class **/
  public constructor(init?: Partial<DropdownMenuItem>) {
    super(MenuItemTypes.Dropdown, init);
  }
}
