import { GenericCrudService } from '../services/backend/crud/genericCrud.service';

export class BaseLookupModel<T = any> {
  lookupName = '';
  lookupDisplayName = '';
  entryDisplayNameFormatter: (data: T) => string;
  service: GenericCrudService<T>;
  columns: ILookupTableField[];
  formFields: ILookupFormField[];
  factory: () => T;

  constructor(init?: Partial<BaseLookupModel>) {
    Object.assign(this, init);
    this.factory = this.factory || (() => <T>{});
  }
}

export class BaseLookupFormField implements ILookupFormField {
  field: string;

  // TODO: Add for boolean and image selector from product lookup
  fieldType: 'number' | 'text' | 'email' | 'select' | 'textarea' = 'text';
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
  fieldType: 'text' | 'number' | 'email' | 'select' | 'textarea';
  label: string;
  options: any;
  placeholder: string;
  required: boolean;
}

export class BaseLookupTableField implements ILookupTableField {
  field: string;
  formatter?: (data: any) => string;

  constructor(init?: Partial<BaseLookupTableField>) {
    Object.assign(this, init);

    this.formatter = this.formatter || ((data: any): string => data[this.field]);
  }
}

export interface ILookupTableField {
  field: string;
  formatter?: (data: any) => string;
}
