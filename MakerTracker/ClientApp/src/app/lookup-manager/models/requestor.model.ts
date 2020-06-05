import { Injectable } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { RequestorReportDto } from 'autogen/RequestorReportDto';
import { BackendService } from 'src/app/services/backend/backend.service';
import { ReadOnlyService } from 'src/app/services/backend/crud/genericCrud.service';
import { ButtonColumnParams } from '../column-types/button-column/button-column-params';
import { BaseLookupModel } from '../lookup-model';

@Injectable()
export class RequestorListModel extends BaseLookupModel<RequestorReportDto> {
  constructor(service: BackendService) {
    super({
      canAdd: false,
      canExport: true,
      canEdit: true,
      canDelete: false,
      lookupName: 'requestors',
      lookupDisplayName: 'Requestors',
      service: <ReadOnlyService<RequestorReportDto>>{
        list: () => service.getAllRequestors(),
        query: () => service.getAllRequestors(),
        get: () => {
          throw new Error('Not Implemented');
        }
      },
      editButtonOverride: <ColDef>{
        cellRendererParams: <ButtonColumnParams>{
          type: 'link',
          route: (params) => ['/', 'profile', params.value],
          color: 'primary',
          tooltip: 'Edit',
          icon: 'edit'
        }
      },
      columns: [
        { headerName: 'Company', field: 'companyName' },
        { headerName: 'First Name', field: 'firstName' },
        { headerName: 'Last Name', field: 'lastName' },
        { headerName: 'Phone #', field: 'phone' },
        { headerName: 'Email', field: 'email' },
        { headerName: 'Address', field: 'address' },
        { headerName: 'City', field: 'city' },
        { headerName: 'State', field: 'state' },
        { headerName: 'Zip', field: 'zipCode' },
        { headerName: 'Created Date', field: 'createdDate' }
      ]
    });
  }
}
