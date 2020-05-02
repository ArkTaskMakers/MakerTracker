import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseLookupModel } from '../lookup-model';
import { ModelProviderService } from '../lookup-model-provider.service';

/** Lists the equipment in the system for admin purposes. */
@Component({
  selector: 'app-lookup-list',
  templateUrl: 'lookup-list.component.html',
  styleUrls: ['./lookup-list.component.scss']
})
export class LookupListComponent implements OnInit {
  /** UI feedback */
  feedback: any = {};

  /** The equipment list */
  data: any[] = [];

  /** The model for generating a lookup editor. */
  model: BaseLookupModel;

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
    this.route.paramMap.subscribe((params) => {
      this.model = modelProvider.models.get(params.get('model'));
      this.refresh();
    });
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {}

  /** refreshes the list of equipment */
  refresh() {
    this.model.service.list().subscribe((res) => (this.data = res));
  }

  /**
   * Deletes the specified entry
   * @param entry the entry to delete
   */
  delete(entry: any): void {
    if (confirm('Are you sure?')) {
      this.model.service.destroy(entry.id).subscribe(
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
