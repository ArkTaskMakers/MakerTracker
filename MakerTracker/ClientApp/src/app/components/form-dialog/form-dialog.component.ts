import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  FormDialogConfig,
  FormDialogImageInputTypeOptions,
  FormDialogModel,
  IFormDialogField
} from './form-dialog-config.model';

@Component({
  selector: 'app-form-dialog',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.css']
})
export class FormDialogComponent implements OnInit {
  /** The id passed by the routing. */
  id: string;

  /** The page title. */
  title: string;

  /** feedback messages for alerting the user. */
  feedback: any = {};

  dialogForm: FormGroup;

  get model(): FormDialogModel {
    return this.config.model;
  }

  alert: { type: string; message: string } = null;

  /**
   * Initializes a new instance of the EquipmentListComponent class.
   * @param route The activated route
   * @param router The angular router for navigation
   * @param _snackBar The snackbar for UI messaging
   * @param _equipmentService The service for interacting with the REST API
   */
  constructor(
    private _snackBar: MatSnackBar,
    private cd: ChangeDetectorRef,
    public dialogRef: MatDialogRef<FormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public config: FormDialogConfig,
    fb: FormBuilder
  ) {
    this.dialogForm = config.model.buildForm(fb);
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {}

  getFieldLabel(field: IFormDialogField) {
    if (typeof field.label === 'string') {
      return field.label;
    }

    return field.label();
  }

  onFileChange(event, field: IFormDialogField) {
    const imageOptions: FormDialogImageInputTypeOptions =
      <FormDialogImageInputTypeOptions>field.options || new FormDialogImageInputTypeOptions();
    const reader = new FileReader();
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.compressImage(reader.result, imageOptions.width || 64, imageOptions.height || 64).then(
          (compressed: string) => {
            this.dialogForm.get(field.field).setValue(compressed);
          },
          (err) => {
            this._snackBar.open('Invalid image', null, {
              duration: 2000
            });
          }
        );

        // need to run CD since file load runs outside of zone
        this.cd.markForCheck();
      };
    }
  }

  compressImage(src, newX, newY) {
    return new Promise((res, rej) => {
      const img = new Image();
      img.src = src;
      img.onload = () => {
        const elem = document.createElement('canvas');
        elem.width = newX;
        elem.height = newY;
        const ctx = elem.getContext('2d');
        ctx.drawImage(img, 0, 0, newX, newY);
        const data = ctx.canvas.toDataURL('image/jpeg', 0.75);
        res(data);
      };
      img.onerror = (error) => rej(error);
    });
  }

  /** Saves the changes to the current entry. */
  save() {
    const data = this.model.onBeforeSubmit(this.dialogForm);
    this.model.onSubmit(data).subscribe(
      (entry) => {
        this._snackBar.open('Save was successful!', null, {
          duration: 2000
        });
        this.dialogRef.close(true);
      },
      (err) => {
        this.alert = {
          type: 'error',
          message: `Error Saving: ${err.message}`
        };
      }
    );
  }

  /** Closes without applying any changes. */
  cancel() {
    this.dialogRef.close(false);
  }
}
