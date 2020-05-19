import { FormBuilder, Validators } from '@angular/forms';
import { NeedDto } from 'autogen/NeedDto';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import {
  FormDialogDateInputOptions,
  FormDialogField,
  FormDialogModel,
  FormDialogNumberInputOptions,
  FormDialogSelectInputOptions,
  FormDialogTextAreaInputOptions,
  IFormDialogField
} from 'src/app/components/form-dialog/form-dialog-config.model';
import { NeedService } from 'src/app/services/backend/crud/need.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { IProductEntry, IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';

export class NeedFormModel extends FormDialogModel<NeedDto> {
  isEditMode = false;
  formFields: IFormDialogField[] = [
    new FormDialogField({
      field: 'productId',
      fieldType: 'select',
      isHidden: () => this.isEditMode,
      label: 'Product',
      required: true,
      options: new FormDialogSelectInputOptions({
        fieldGroups: of(this.products),
        getOptionValue: (product) => product.id,
        getOptionDisplay: (product) => product.name,
        getOptionGroupOptions: (group) => group.products,
        getOptionGroupDisplay: (group) => group.name
      })
    }),
    new FormDialogField({
      field: 'productId',
      fieldType: 'description',
      label: () => "What's the new amount for " + this.getProductLabel() + '?',
      isHidden: () => !this.isEditMode
    }),
    new FormDialogField({
      field: 'dueDate',
      fieldType: 'date',
      label: 'Due Date',
      options: new FormDialogDateInputOptions({
        min: new Date()
      })
    }),
    new FormDialogField({
      field: 'specialInstructions',
      fieldType: 'textarea',
      label: 'Special Instructions?',
      options: new FormDialogTextAreaInputOptions()
    }),
    new FormDialogField({
      field: 'quantity',
      fieldType: 'number',
      label: 'Amount',
      required: true,
      options: new FormDialogNumberInputOptions({
        step: 1,
        min: 0
      })
    })
  ];
  constructor(
    entry: NeedDto,
    protected service: NeedService,
    protected products: IProductTypeGroup[],
    protected productMap: Map<number, IProductEntry>,
    init?: Partial<FormDialogModel<NeedDto>>
  ) {
    super(entry, init);
    this.isEditMode = !!entry;
  }

  buildForm(fb: FormBuilder) {
    const data = { ...this.entry };
    return fb.group({
      id: (data && data.id) || 0,
      profileId: (data && data.profileId) || 0,
      createdDate: (data && data.createdDate) || new Date(),
      fulfilledDate: data && data.fulfilledDate,
      productId: [data && data.productId, [Validators.required]],
      quantity: [(data && data.quantity) || 0, [Validators.required, Validators.min(0)]],
      specialInstructions: data && data.specialInstructions,
      dueDate: data && data.dueDate
    });
  }

  onSubmit(data: NeedDto): Observable<NeedDto> {
    return this.isEditMode ? this.service.update(data.id, data) : this.service.create(data);
  }

  getProductLabel() {
    const product = this.productMap.get(this.entry.productId);
    return product && product.name;
  }
}
