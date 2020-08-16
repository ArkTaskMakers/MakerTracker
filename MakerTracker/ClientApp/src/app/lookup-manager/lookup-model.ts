import { ColDef } from 'ag-grid-community';
import { GenericCrudService, ReadOnlyService } from '../services/backend/crud/genericCrud.service';

export type LookupInputType =
  | 'number'
  | 'text'
  | 'email'
  | 'select'
  | 'textarea'
  | 'bool-toggle'
  | 'image'
  | 'description';

export class BaseLookupModel<T = any> {
  lookupName = '';
  lookupDisplayName = '';
  entryDisplayNameFormatter: (data: T) => string;
  service: ReadOnlyService<T> | GenericCrudService<T>;
  canAdd = false;
  canExport = false;
  canEdit = false;
  canDelete = false;
  columns: ColDef[];

  editButtonOverride?: ColDef;
  deleteButtonOverride?: ColDef;

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
