import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';
import { InventoryTransactionDto } from 'autogen/InventoryTransactionDto';
import { NeedLookupDto } from 'autogen/NeedLookupDto';
import { ProductDto } from 'autogen/ProductDto';
import { Observable, Subject } from 'rxjs';
import {
  FormDialogField,
  FormDialogModel,
  FormDialogNumberInputOptions,
  FormDialogSelectInputOptions,
  IFormDialogField
} from 'src/app/components/form-dialog/form-dialog-config.model';
import { BackendService } from 'src/app/services/backend/backend.service';
import { NeedService } from 'src/app/services/backend/crud/need.service';
import { IProductEntry } from 'src/app/ui-models/productTypeGroup';

export class DeliveryFormModel extends FormDialogModel<InventoryTransactionDto> {
  needSubject: Subject<NeedLookupDto[]> = new Subject<NeedLookupDto[]>();
  isEditMode = false;
  formFields: IFormDialogField[] = [
    new FormDialogField({
      field: 'need',
      fieldType: 'select',
      placeholder: 'Select the need to fulfill',
      options: new FormDialogSelectInputOptions({
        compareWith: (o1, o2) => (o1 && o1.needId) === (o2 && o2.needId),
        fieldOptions: this.needSubject,
        getOptionDisplay: (need: NeedLookupDto) => {
          const product = this.productMap.get(need.productId);
          return (
            `${product.name} - ${need.profileDisplayName} lacks ${need.outstandingQuantity}` +
            (!!need.dueDate ? ` by ${new Date(need.dueDate).toLocaleDateString()}` : '')
          );
        }
      })
    }),
    new FormDialogField({
      field: 'amount',
      fieldType: 'number',
      label: () => `Amount (out of ${this.product.amount} on-hand)`,
      options: new FormDialogNumberInputOptions({
        step: 1,
        min: 1
      })
    })
  ];
  constructor(
    protected product: InventoryProductSummaryDto,
    protected service: BackendService,
    protected productMap: Map<number, IProductEntry>,
    protected needSvc: NeedService,
    init?: Partial<FormDialogModel<InventoryTransactionDto>>
  ) {
    super(new InventoryTransactionDto(), init);
    needSvc.listOutstanding(product.productId).subscribe((needs) => {
      this.needSubject.next(needs);
    });
  }

  buildForm(fb: FormBuilder) {
    return fb.group({
      need: [null, [Validators.required]],
      amount: [0, [Validators.required, Validators.min(1), Validators.max(this.product.amount)]]
    });
  }

  onBeforeSubmit(form: FormGroup) {
    const formValue = form.value;
    const need = <NeedLookupDto>formValue.need;
    const product = this.productMap.get(need.productId);
    return new InventoryTransactionDto({
      amount: formValue.amount,
      needId: need.needId,
      product: new ProductDto({
        id: product.id,
        name: product.name,
        imageUrl: product.imageUrl
      })
    });
  }

  onSubmit(data: InventoryTransactionDto): Observable<InventoryTransactionDto> {
    return this.service.saveInventory(data);
  }
}
