import { Injectable } from '@angular/core';
import { ProductDto } from 'autogen/ProductDto';
import { ProductTypeDto } from 'autogen/ProductTypeDto';
import { shareReplay } from 'rxjs/operators';
import { ProductService } from 'src/app/services/backend/crud/product.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { BaseLookupFormField, BaseLookupModel } from '../lookup-model';

@Injectable()
export class ProductModel extends BaseLookupModel<ProductDto> {
  productMap: Map<number, ProductTypeDto> = new Map<number, ProductTypeDto>();

  constructor(service: ProductService, typeSvc: ProductTypeService) {
    super({
      canAdd: true,
      canExport: true,
      canEdit: true,
      canDelete: true,
      lookupName: 'products',
      lookupDisplayName: 'Products',
      entryDisplayNameFormatter: (data) => data.name,
      service
    });

    typeSvc.list().subscribe((types) => {
      this.productMap = new Map(types.map((e) => [e.id, e] as [number, ProductTypeDto]));
    });

    this.columns = [
      { headerName: 'Name', field: 'name', sortable: true, filter: true },
      { headerName: 'Description', field: 'description', sortable: true, filter: true },
      {
        headerName: 'Product Type',
        field: 'productTypeId',
        sortable: true,
        filter: true,
        valueFormatter: (valueParams) => this.productMap.get(valueParams.value).name
      }
    ];

    this.formFields = [
      new BaseLookupFormField({
        field: 'name',
        fieldType: 'text',
        label: 'Name',
        required: true,
        options: {
          maxlength: 100
        }
      }),
      new BaseLookupFormField({
        field: 'description',
        fieldType: 'textarea',
        label: 'Description',
        required: true,
        options: {
          maxlength: 500
        }
      }),
      new BaseLookupFormField({
        field: 'productTypeId',
        fieldType: 'select',
        placeholder: 'Product Type',
        options: {
          fieldOptions: typeSvc.lookup().pipe(shareReplay(1)),
          getOptionValue: (option: ProductTypeDto) => option.id,
          getOptionDisplay: (option: ProductTypeDto) => option.name
        },
        required: true
      }),
      new BaseLookupFormField({
        field: 'instructionUrl',
        fieldType: 'text',
        label: 'Instruction Url',
        options: {
          maxlength: 255
        }
      }),
      new BaseLookupFormField({
        field: 'imageUrl',
        fieldType: 'image',
        label: 'Image Url',
        options: {
          width: 64,
          height: 64
        }
      }),
      new BaseLookupFormField({
        field: 'isDeprecated',
        fieldType: 'bool-toggle',
        label: 'Deprecated?'
      })
    ];
  }
}
