import { Injectable } from '@angular/core';
import { ProductDto } from 'autogen/ProductDto';
import { ProductTypeDto } from 'autogen/ProductTypeDto';
import { shareReplay } from 'rxjs/operators';
import { ProductService } from 'src/app/services/backend/crud/product.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { BaseLookupFormField, BaseLookupModel } from '../lookup-model';

@Injectable()
export class ProductModel extends BaseLookupModel<ProductDto> {
  constructor(service: ProductService, typeSvc: ProductTypeService) {
    super({
      lookupName: 'products',
      lookupDisplayName: 'Products',
      entryDisplayNameFormatter: (data) => data.name,
      service,
      nameField: 'name',
      descriptionField: 'description',
      imageField: 'imageUrl'
    });

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
