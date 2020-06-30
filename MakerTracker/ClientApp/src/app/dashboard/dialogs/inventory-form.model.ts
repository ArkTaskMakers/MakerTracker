import { FormBuilder, Validators } from '@angular/forms';
import { InventoryTransactionDto } from 'autogen/InventoryTransactionDto';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  FormDialogField,
  FormDialogModel,
  FormDialogNumberInputOptions,
  FormDialogSelectInputOptions,
  IFormDialogField
} from 'src/app/components/form-dialog/form-dialog-config.model';
import { BackendService } from 'src/app/services/backend/backend.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { IProductEntry, IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';

export class InventoryFormModel extends FormDialogModel<InventoryTransactionDto> {
  isEditMode = false;
  formFields: IFormDialogField[] = [
    new FormDialogField({
      field: 'product',
      fieldType: 'select',
      isHidden: () => this.isEditMode,
      placeholder: 'Product',
      options: new FormDialogSelectInputOptions({
        compareWith: (o1, o2) => (o1 && o1.id) === (o2 && o2.id),
        fieldGroups: this.productSvc
          .getProductHierarchy()
          .pipe(map((data) => data && data.filter((g) => g && g.products && g.products.some((p) => !p.isDeprecated)))),
        getOptionValue: (product: IProductEntry) => product,
        getOptionDisplay: (product: IProductEntry) => product.name,
        getOptionGroupOptions: (group: IProductTypeGroup) => group.products.filter((p) => !p.isDeprecated),
        getOptionGroupDisplay: (group: IProductTypeGroup) => group.name
      })
    }),
    new FormDialogField({
      field: 'product',
      fieldType: 'description',
      label: `What's the new amount for ${this.entry && this.entry.product && this.entry.product.name}?`,
      isHidden: () => !this.isEditMode
    }),
    new FormDialogField({
      field: 'amount',
      fieldType: 'number',
      label: 'Amount',
      options: new FormDialogNumberInputOptions({
        step: 1,
        min: 0
      })
    })
  ];
  constructor(
    entry: InventoryTransactionDto,
    protected service: BackendService,
    protected productSvc: ProductTypeService,
    init?: Partial<FormDialogModel<InventoryTransactionDto>>
  ) {
    super(entry, init);
    this.isEditMode = !!entry;
  }

  buildForm(fb: FormBuilder) {
    const data = { ...this.entry };
    return fb.group({
      product: [data && data.product, [Validators.required]],
      amount: [(data && data.amount) || 0, [Validators.required, Validators.min(0)]]
    });
  }

  onSubmit(data: InventoryTransactionDto): Observable<InventoryTransactionDto> {
    return this.isEditMode ? this.service.editInventory(data) : this.service.saveInventory(data);
  }
}
