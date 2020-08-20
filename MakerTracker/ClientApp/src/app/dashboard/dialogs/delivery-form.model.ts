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
  formData: FormGroup;
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
    }),
    new FormDialogField({
      fieldType: 'description',
      label: () => {
        const need: NeedLookupDto = this.formData.get('need').value;
        if (!need) {
          return null;
        }
        if (!need.isDropOffPoint) {
          return `<p>This requestor is not marked as a public dropoff point, therefore we are not supplying their location information. Pleast contact this user directly via our Slack.</p>`;
        }
        const address = `<p><address>${[need.address, need.address2].filter((a) => !!a).join('<br />')}<br />${
          need.city
        }, ${need.state} ${need.zipCode}</address></p>`;
        const due = need.dueDate
          ? `<p>It needs to be delivered by ${new Date(need.dueDate).toLocaleDateString()} at the latest.</p>`
          : '';
        const contact = `<p>If you have any questions, please contact <strong>${need.profileDisplayName}</strong> at <a href="mailto:${need.contactEmail}">${need.contactEmail}</a></p>`;
        return `<p>Thank you for delivering this PPE! This organization's name is ${need.profileDisplayName}. Their address is as follows:</p>${address}${due}${contact}`;
      }
    }),
    new FormDialogField({
      fieldType: 'description',
      label: () => {
        const need: NeedLookupDto = this.formData.get('need').value;
        if (!need || !need.specialInstructions) {
          return null;
        }
        return `<p><strong>Special Instructions from requestor:</strong><br />${need.specialInstructions}</p>`;
      }
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
    this.formData = fb.group({
      need: [null, [Validators.required]],
      amount: [0, [Validators.required, Validators.min(1), Validators.max(this.product.amount)]]
    });
    return this.formData;
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
