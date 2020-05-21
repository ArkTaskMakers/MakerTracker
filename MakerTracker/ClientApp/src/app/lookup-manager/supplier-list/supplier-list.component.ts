import { Component, OnInit } from '@angular/core';
import { BackendService } from 'src/app/services/backend/backend.service';

@Component({
  selector: 'app-supplier-list',
  templateUrl: './supplier-list.component.html',
  styleUrls: ['./supplier-list.component.scss']
})
export class SupplierListComponent implements OnInit {

  private gridApi;
  private gridColumnApi;

  columnDefs = [
    { headerName: 'Company', field: 'companyName', sortable: true, filter: true },
    { headerName: 'First Name', field: 'firstName', sortable: true, filter: true },
    { headerName: 'Last Name', field: 'lastName', sortable: true, filter: true },
    { headerName: 'Phone #', field: 'phone', sortable: true, filter: true },
    { headerName: 'Email', field: 'email', sortable: true, filter: true },
    { headerName: 'Address', field: 'address', sortable: true, filter: true },
    { headerName: 'City', field: 'city', sortable: true, filter: true },
    { headerName: 'State', field: 'state', sortable: true, filter: true },
    { headerName: 'Zip', field: 'zipCode', sortable: true, filter: true },
    { headerName: 'Created Date', field: 'createdDate', sortable: true, filter: true },

  ];

  rowData: any;

  constructor(private backendService: BackendService) { }

  ngOnInit(): void {
    this.rowData = this.backendService.getAllSuppliers();
  }

  exportCSV(){
    this.gridApi.exportDataAsCsv();
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }


}
