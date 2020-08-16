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
import { IProductEntry, IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';

export class NeedFormModel extends FormDialogModel<NeedDto> {
  isEditMode = false;
  formFields: IFormDialogField[] = [
    new FormDialogField({
      fieldType: 'description',
      label: 'Please include both the PPE products and the number you need from suppliers.'
    }),
    new FormDialogField({
      fieldType: 'description',
      label: "Don't see the PPE you need? Email us an example product at pgordon@arhub.org."
    }),
    new FormDialogField({
      field: 'productId',
      fieldType: 'select',
      isHidden: () => this.isEditMode,
      placeholder: 'Product',
      required: true,
      options: new FormDialogSelectInputOptions({
        fieldGroups: of(this.products.filter((g) => g && g.products && g.products.some((p) => !p.isDeprecated))),
        getOptionValue: (product: IProductEntry) => product.id,
        getOptionDisplay: (product: IProductEntry) => product.name,
        getOptionGroupOptions: (group: IProductTypeGroup) => group.products.filter((p) => !p.isDeprecated),
        getOptionGroupDisplay: (group: IProductTypeGroup) => group.name
      })
    }),
    new FormDialogField({
      field: 'productId',
      fieldType: 'description',
      label: () => `What's the new amount for ${this.getProductLabel()}?`,
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
