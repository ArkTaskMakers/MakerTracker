import { Injectable } from '@angular/core';
import { ProductTypeDto } from 'autogen/ProductTypeDto';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { BaseLookupFormField, BaseLookupModel } from '../lookup-model';

@Injectable()
export class ProductTypeModel extends BaseLookupModel<ProductTypeDto> {
  constructor(service: ProductTypeService) {
    super({
      canAdd: true,
      canExport: true,
      canEdit: true,
      canDelete: true,
      lookupName: 'product-type',
      lookupDisplayName: 'Product Types',
      entryDisplayNameFormatter: (data) => data.name,
      service,
      formFields: [
        new BaseLookupFormField({
          field: 'name',
          fieldType: 'text',
          label: 'Name',
          placeholder: 'Product type name',
          required: true,
          options: {
            maxlength: 100
          }
        }),
        new BaseLookupFormField({
          field: 'sortOrder',
          fieldType: 'number',
          label: 'Sort Order',
          placeholder: 'Input the sorting order for the type',
          required: true
        })
      ],
      columns: [
        { headerName: 'Name', field: 'name', sortable: true, filter: true },
        { headerName: 'Sort Order', field: 'sortOrder', sortable: true, filter: true }
      ]
    });
  }
}
