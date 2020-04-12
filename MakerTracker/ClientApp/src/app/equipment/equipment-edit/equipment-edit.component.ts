import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentDto } from 'autogen/EquipmentDto';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';

/** The component for editing equipment */
@Component({
  selector: 'app-equipment-edit',
  templateUrl: './equipment-edit.component.html',
  styleUrls: ['./equipment-edit.component.scss']
})
export class EquipmentEditComponent implements OnInit {

  /** The id passed by the routing. */
  id: string;

  /** The entry currently being edited. */
  entry: EquipmentDto;

  /** feedback messages for alerting the user. */
  feedback: any = {};

  /**
   * Initializes a new instance of the EquipmentListComponent class.
   * @param route The activated route
   * @param router The angular router for navigation
   * @param _snackBar The snackbar for UI messaging
   * @param _equipmentService The service for interacting with the REST API
   */
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private _snackBar: MatSnackBar,
    private _equipmentService: EquipmentService) {
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {
    this
      .route
      .params
      .pipe(
        map(p => p.id),
        switchMap(id => {
          if (id === 'new') { return of(new EquipmentDto()); }
          return this._equipmentService.get(id);
        })
      )
      .subscribe(entry => {
        this.entry = entry;
        this.feedback = {};
      },
        err => {
          this._snackBar.open('Error loading', null, {
            duration: 2000,
          });
        }
      );
  }

  /** Saves the changes to the current entry. */
  save() {
    const request = this.entry.id ? this._equipmentService.update(this.entry.id, this.entry) : this._equipmentService.create(this.entry);
    request.subscribe(
      entry => {
        this.entry = entry;
        this._snackBar.open('Save was successful!', null, {
          duration: 2000,
        });
        this.router.navigate(['/equipment']);
      },
      err => {
        this._snackBar.open('Error Saving', null, {
          duration: 2000,
        });
      }
    );
  }

  /** Navigates back to the equipment list without applying any changes. */
  cancel() {
    this.router.navigate(['/equipment']);
  }
}
