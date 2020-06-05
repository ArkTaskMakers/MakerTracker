import { Injectable } from '@angular/core';
import { EquipmentDto } from 'autogen/EquipmentDto';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';
import { BaseLookupFormField, BaseLookupModel } from '../lookup-model';

@Injectable()
export class EquipmentModel extends BaseLookupModel<EquipmentDto> {
  constructor(service: EquipmentService) {
    super({
      canAdd: true,
      canExport: true,
      canEdit: true,
      canDelete: true,
      lookupName: 'equipment',
      lookupDisplayName: 'Equipment',
      entryDisplayNameFormatter: (data) => data.name,
      service,
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
      ],
      columns: [
        { headerName: 'Name', field: 'name', sortable: true, filter: true },
        { headerName: 'Description', field: 'description', sortable: true, filter: true }
      ]
    });
  }
}
