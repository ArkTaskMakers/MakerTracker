import { GenericCrudService } from '../services/backend/crud/genericCrud.service';

export type LookupInputType = 'number' | 'text' | 'email' | 'select' | 'textarea' | 'bool-toggle' | 'image';

export class BaseLookupModel<T = any> {
  lookupName = '';
  lookupDisplayName = '';
  entryDisplayNameFormatter: (data: T) => string;
  service: GenericCrudService<T>;
  nameField = 'name';
  descriptionField: string;
  imageField: string;

  formFields: ILookupFormField[];
  factory: () => T;

  constructor(init?: Partial<BaseLookupModel>) {
    Object.assign(this, init);
    this.factory = this.factory || (() => <T>{});
  }
}

export class BaseLookupFormField implements ILookupFormField {
  field: string;
  fieldType: LookupInputType = 'text';
  label: string;
  options: any = null;
  placeholder: string = null;
  required = false;

  constructor(init?: Partial<BaseLookupFormField>) {
    Object.assign(this, init);
  }
}

export interface ILookupFormField {
  field: string;
  fieldType: LookupInputType;
  label: string;
  options: any;
  placeholder: string;
  required: boolean;
}
