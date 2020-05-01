import { Injectable } from '@angular/core';
import { EquipmentDto } from 'autogen/EquipmentDto';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';
import { BaseLookupFormField, BaseLookupModel, BaseLookupTableField } from '../lookup-model';

@Injectable()
export class EquipmentModel extends BaseLookupModel<EquipmentDto> {
  constructor(service: EquipmentService) {
    super({
      lookupName: 'equipment',
      lookupDisplayName: 'Equipment',
      entryDisplayNameFormatter: (data) => data.name,
      service,
      columns: [
        new BaseLookupTableField({
          field: 'name'
        })
      ],
      formFields: [
        new BaseLookupFormField({
          field: 'name',
          fieldType: 'text',
          label: 'Name',
          placeholder: 'Equipment name',
          required: true,
          options: {
            maxlength: 100
          }
        }),
        new BaseLookupFormField({
          field: 'description',
          fieldType: 'textarea',
          label: 'Description',
          placeholder: 'Description',
          required: true,
          options: {
            maxlength: 500
          }
        })
      ]
    });
  }
}
