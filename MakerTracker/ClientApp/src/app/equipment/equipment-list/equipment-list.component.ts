import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';
import { EquipmentDto } from 'autogen/EquipmentDto';

/** Lists the equipment in the system for admin purposes. */
@Component({
  selector: 'app-equipment',
  templateUrl: 'equipment-list.component.html',
  styleUrls: ['./equipment-list.component.scss']
})
export class EquipmentListComponent implements OnInit {

  /** UI feedback */
  feedback: any = {};

  /** The equipment list */
  equipment: EquipmentDto[] = [];

  /**
   * Initializes a new instance of the EquipmentListComponent class.
   * @param service The equipment service to interact with the REST API
   * @param _snackBar The snackbar for presenting UI messages
   */
  constructor(private service: EquipmentService, private _snackBar: MatSnackBar) {
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {
    this.refresh();
  }

  /** refreshes the list of equipment */
  refresh() {
    this.service.list().subscribe(res => this.equipment = res);
  }

  /**
   * Deletes the specified entry
   * @param entry the entry to delete
   */
  delete(entry: EquipmentDto): void {
    if (confirm('Are you sure?')) {
      this.service.destroy(entry.id).subscribe(() => {
        this.refresh();
        this._snackBar.open('Delete was successful', null, {
          duration: 2000,
        });
      },
        err => {
          this._snackBar.open('Error Deleting', null, {
            duration: 2000,
          });
        }
      );
    }
  }
}
