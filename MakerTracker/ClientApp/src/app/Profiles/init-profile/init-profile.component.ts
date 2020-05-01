import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ProfileDto } from 'autogen/ProfileDto';
import { BackendService } from 'src/app/services/backend/backend.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { IState, StatesService } from 'src/app/services/states.service';
import { IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';

@Component({
  selector: 'app-init-profile',
  templateUrl: './init-profile.component.html',
  styleUrls: ['./init-profile.component.scss']
})
export class InitProfileComponent implements OnInit {
  get isSupplier(): boolean {
    return <boolean>this.firstFormGroup.get('isSupplier').value;
  }
  set isSupplier(value: boolean) {
    this.firstFormGroup.get('isSupplier').setValue(value);
  }
  get isRequestor(): boolean {
    return <boolean>this.firstFormGroup.get('isRequestor').value;
  }
  set isRequestor(value: boolean) {
    this.firstFormGroup.get('isRequestor').setValue(value);
  }
  get isNeither(): boolean {
    return <boolean>this.firstFormGroup.get('isNeither').value;
  }
  set isNeither(value: boolean) {
    this.firstFormGroup.get('isNeither').setValue(value);
  }

  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  supplierFormGroup: FormGroup;
  requestorFormGroup: FormGroup;

  states: IState[];
  products: IProductTypeGroup[];

  constructor(
    public dialogRef: MatDialogRef<InitProfileComponent>,
    private router: Router,
    private backend: BackendService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    productTypesSvc: ProductTypeService,
    stateSvc: StatesService
  ) {
    this.firstFormGroup = this.fb.group(
      {
        isSupplier: [false],
        isRequestor: [false],
        isNeither: [false]
      },
      { validators: this.verifyRoles }
    );
    this.secondFormGroup = this.fb.group({
      city: ['', Validators.required],
      state: ['AR', Validators.required],
      zipCode: ['', Validators.required]
    });
    this.supplierFormGroup = this.fb.group({
      products: [[]]
    });
    this.requestorFormGroup = this.fb.group({
      secondCtrl: ['', Validators.required]
    });
    this.states = stateSvc.states;
    productTypesSvc.getProductHierarchy().subscribe((products) => {
      this.products = products;
    });
  }

  ngOnInit() {}

  closeDialog() {
    if (this.isSupplier || this.isRequestor || this.isNeither) {
      this.dialogRef.close(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor
        })
      );
    }
  }

  refreshNeither() {
    if (this.isSupplier || this.isRequestor) {
      this.isNeither = false;
    }
  }

  refreshOthers() {
    if (this.isNeither) {
      this.isSupplier = false;
      this.isRequestor = false;
    }
  }

  verifyRoles: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    if (!this.firstFormGroup) {
      return null;
    }
    return !this.isSupplier && !this.isRequestor && !this.isNeither ? { lackingRoles: true } : null;
  };

  submit() {
    this.backend
      .saveProfile(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor
        })
      )
      .subscribe(
        () => {
          this.closeDialog();
          this.router.navigate(['/dashboard']);
        },
        (err) => {
          this.snackBar.open('Error', undefined, {
            duration: 3000
          });
        }
      );
  }
}
