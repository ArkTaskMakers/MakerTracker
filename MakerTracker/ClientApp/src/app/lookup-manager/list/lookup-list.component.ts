import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridOptions } from 'ag-grid-community';
import { GenericCrudService } from 'src/app/services/backend/crud/genericCrud.service';
import { ButtonColumnParams } from '../column-types/button-column/button-column-params';
import { ButtonColumnComponent } from '../column-types/button-column/button-column.component';
import { BaseLookupModel } from '../lookup-model';
import { ModelProviderService } from '../lookup-model-provider.service';

/** Lists the equipment in the system for admin purposes. */
@Component({
  selector: 'app-lookup-list',
  templateUrl: 'lookup-list.component.html',
  styleUrls: ['./lookup-list.component.scss']
})
export class LookupListComponent implements OnInit {
  @ViewChild('grid') grid: AgGridAngular;

  /** UI feedback */
  feedback: any = {};

  /** The equipment list */
  data: any[] = [];

  /** The model for generating a lookup editor. */
  model: BaseLookupModel;

  /** The columns the grid is bound to. */
  columns: ColDef[];

  gridOptions = <GridOptions>{
    frameworkComponents: {
      buttonColumn: ButtonColumnComponent
    },
    defaultColDef: {
      sortable: true,
      filter: true
    }
  };

  /**
   * Initializes a new instance of the EquipmentListComponent class.
   * @param service The equipment service to interact with the REST API
   * @param _snackBar The snackbar for presenting UI messages
   */
  constructor(
    private route: ActivatedRoute,
    private _snackBar: MatSnackBar,
    router: Router,
    modelProvider: ModelProviderService
  ) {
    this.route.paramMap.subscribe((routeParams) => {
      this.model = modelProvider.models.get(routeParams.get('model'));
      this.refresh();
      this.columns = [...this.model.columns];
      if (this.model.canEdit) {
        this.columns.push(<ColDef>{
          headerName: '',
          width: 52,
          field: 'id',
          colId: 'edit',
          cellRenderer: 'buttonColumn',
          sortable: false,
          filter: false,
          pinned: 'left',
          cellClass: 'compact',
          cellRendererParams: <ButtonColumnParams>{
            type: 'link',
            route: (params) => ['.', params.value],
            color: 'primary',
            tooltip: 'Edit',
            icon: 'edit'
          },
          ...this.model.editButtonOverride
        });
      }

      if (this.model.canDelete) {
        this.columns.push(<ColDef>{
          headerName: '',
          width: 52,
          field: 'id',
          colId: 'delete',
          cellRenderer: 'buttonColumn',
          sortable: false,
          filter: false,
          pinned: 'left',
          cellClass: 'compact',
          cellRendererParams: <ButtonColumnParams>{
            type: 'action',
            action: (params) => this.delete(params.data),
            color: 'error',
            tooltip: 'Delete',
            icon: 'delete_forever'
          },
          ...this.model.deleteButtonOverride
        });
      }
    });
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {}

  /** refreshes the list of data */
  refresh() {
    this.data = [];
    this.model.service.list().subscribe((res) => (this.data = res));
  }

  exportCsv() {
    this.grid.api.exportDataAsCsv({
      columnKeys: this.model.columns.map((c) => c.field)
    });
  }

  /**
   * Deletes the specified entry
   * @param entry the entry to delete
   */
  delete(entry: any): void {
    if (confirm('Are you sure?')) {
      (<GenericCrudService<any>>this.model.service).destroy(entry.id).subscribe(
        () => {
          this.refresh();
          this._snackBar.open('Delete was successful', null, {
            duration: 2000
          });
        },
        (err) => {
          this._snackBar.open('Error Deleting', null, {
            duration: 2000
          });
        }
      );
    }
  }
}
