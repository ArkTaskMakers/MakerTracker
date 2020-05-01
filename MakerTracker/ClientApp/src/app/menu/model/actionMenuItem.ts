import { BaseMenuItem } from './baseMenuItem';
import { MenuItemTypes } from './menuItemTypes.enum';

export class ActionMenuItem extends BaseMenuItem {
  text: string;
  action: Function;

  /** Initializes a new instance of the ActionMenuItem class **/
  public constructor(init?: Partial<ActionMenuItem>) {
    super(MenuItemTypes.Action, init);
  }
}
