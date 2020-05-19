import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';

export type FormDialogInputTypeOptions =
  | FormDialogSelectInputOptions
  | FormDialogTextInputOptions
  | FormDialogTextAreaInputOptions
  | FormDialogNumberInputOptions
  | FormDialogImageInputTypeOptions
  | FormDialogDateInputOptions;

export type FormDialogInputType =
  | 'number'
  | 'text'
  | 'email'
  | 'select'
  | 'textarea'
  | 'bool-toggle'
  | 'image'
  | 'description'
  | 'date';

export class FormDialogConfig<T = any> {
  model: FormDialogModel;
  title: string;

  constructor(init?: Partial<FormDialogConfig>) {
    Object.assign(this, init);
  }
}

export abstract class FormDialogModel<T = any> {
  abstract formFields: IFormDialogField[];

  constructor(public entry: T, init?: Partial<FormDialogModel>) {
    Object.assign(this, init);
  }

  abstract buildForm(fb: FormBuilder): FormGroup;
  abstract onSubmit(data: T): Observable<T>;

  onBeforeSubmit(form: FormGroup): T {
    return form.value;
  }

  onLoad(form: FormGroup): void {}
}

export class FormDialogField implements IFormDialogField {
  field: string;
  fieldType: FormDialogInputType = 'text';
  label: string | (() => string);
  options: FormDialogInputTypeOptions = null;
  placeholder: string = null;
  required = false;
  isHidden = (form: FormGroup) => !form;

  constructor(init?: Partial<FormDialogField>) {
    Object.assign(this, init);
  }
}

export class FormDialogSelectInputOptions {
  multiple = false;
  fieldOptions?: Observable<any[]>;
  fieldGroups?: Observable<any[]>;
  compareWith = (o1: any, o2: any): boolean => o1 === o2;
  getOptionValue = (value: any): any => value;
  getOptionDisplay = (value: any): string => value;
  getOptionGroupDisplay = (group: any): string => {
    console.error(`Unable to configure dropdown group name, requires custom option group name provider.`);
    return null;
  };
  getOptionGroupOptions = (group: any): any[] => {
    console.error(`Unable to configure dropdown group options, requires custom option group children provider.`);
    return [];
  };

  constructor(init?: Partial<FormDialogSelectInputOptions>) {
    Object.assign(this, init);
  }
}

export class FormDialogTextInputOptions {
  maxLength: number = null;

  constructor(init?: Partial<FormDialogTextInputOptions>) {
    Object.assign(this, init);
  }
}

export class FormDialogTextAreaInputOptions extends FormDialogTextInputOptions {
  rows = 4;

  constructor(init?: Partial<FormDialogTextAreaInputOptions>) {
    super(init);
  }
}

export class FormDialogImageInputTypeOptions {
  width = 40;
  height = 40;

  constructor(init?: Partial<FormDialogImageInputTypeOptions>) {
    Object.assign(this, init);
  }
}

export class FormDialogNumberInputOptions {
  min: number = null;
  max: number = null;
  step = 1;

  constructor(init?: Partial<FormDialogNumberInputOptions>) {
    Object.assign(this, init);
  }
}

export class FormDialogDateInputOptions {
  min: Date = null;
  max: Date = null;

  constructor(init?: Partial<FormDialogDateInputOptions>) {
    Object.assign(this, init);
  }
}

export interface IFormDialogField {
  field: string;
  fieldType: FormDialogInputType;
  label: string | (() => string);
  options?: FormDialogInputTypeOptions;
  placeholder: string;
  required: boolean;
  isHidden: (form: FormGroup) => boolean;
}
