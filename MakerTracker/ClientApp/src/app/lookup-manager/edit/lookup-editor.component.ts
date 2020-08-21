import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { GenericCrudService } from 'src/app/services/backend/crud/genericCrud.service';
import { BaseLookupModel, ILookupFormField } from '../lookup-model';
import { ModelProviderService } from '../lookup-model-provider.service';

/** The component for editing equipment */
@Component({
  selector: 'app-lookup-editor',
  templateUrl: './lookup-editor.component.html',
  styleUrls: ['./lookup-editor.component.scss']
})
export class LookupEditorComponent implements OnInit {
  /** The id passed by the routing. */
  id: string;

  /** The page title. */
  title: string;

  /** The entry currently being edited. */
  entry: any;

  /** The model for generating a lookup editor. */
  model: BaseLookupModel;

  /** feedback messages for alerting the user. */
  feedback: any = {};

  /** indicates a request is loading. */
  isLoading = false;

  /**
   * Initializes a new instance of the EquipmentListComponent class.
   * @param route The activated route
   * @param router The angular router for navigation
   * @param _snackBar The snackbar for UI messaging
   * @param modelProvider The provider for getting lookup models
   * @param cd The angular change detector
   */
  constructor(
    protected route: ActivatedRoute,
    private router: Router,
    private _snackBar: MatSnackBar,
    modelProvider: ModelProviderService,
    private cd: ChangeDetectorRef
  ) {
    this.route.paramMap.subscribe((params) => {
      this.entry = null;
      this.model = modelProvider.models.get(params.get('model'));
      const id = <any>params.get('id');
      (id === 'new' ? of((this.model.factory && this.model.factory()) || {}) : this.model.service.get(id)).subscribe(
        (entry) => {
          this.entry = entry;
          this.feedback = {};
          this.title = entry.id
            ? `Editing '${this.model.entryDisplayNameFormatter(entry)}'`
            : `Add new ${this.model.lookupDisplayName}`;
        },
        () => {
          this._snackBar.open('Error loading', null, {
            duration: 2000
          });
        }
      );
    });
  }

  /** Hooks into the OnInit lifetime event. */
  ngOnInit() {}

  onFileChange(event, field: ILookupFormField) {
    field.options = field.options || {};
    const reader = new FileReader();
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.compressImage(<string>reader.result, field.options.width || 64, field.options.height || 64).then(
          (compressed: string) => {
            this.entry[field.field] = compressed;
          },
          () => {
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

  compressImage(src: string, newX: number, newY: number): Promise<string> {
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
    const service = <GenericCrudService<any>>this.model.service;
    const request = this.entry.id ? service.update(this.entry.id, this.entry) : service.create(this.entry);
    this.isLoading = true;
    request.subscribe(
      (entry) => {
        this.isLoading = false;
        this.entry = entry;
        this._snackBar.open('Save was successful!', null, {
          duration: 2000
        });
        this.router.navigate(['admin', this.model.lookupName]);
      },
      () => {
        this.isLoading = false;
        this._snackBar.open('Error Saving', null, {
          duration: 2000
        });
      }
    );
  }

  /** Navigates back to the equipment list without applying any changes. */
  cancel() {
    this.router.navigate(['admin', this.model.lookupName]);
  }
}
