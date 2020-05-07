import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { MakerEquipmentDto } from 'autogen/MakerEquipmentDto';
import { ProfileDto } from 'autogen/ProfileDto';
import { of, zip } from 'rxjs';
import { flatMap, toArray } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BackendService } from 'src/app/services/backend/backend.service';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';
import { MakerEquipmentService } from 'src/app/services/backend/crud/makerEquipment.service';
import { ProductTypeService } from 'src/app/services/backend/crud/productType.service';
import { IState, StatesService } from 'src/app/services/states.service';
import { IProductEntry, IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';

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
  locationFormGroup: FormGroup;
  supplierFormGroup: FormGroup;
  requestorFormGroup: FormGroup;
  productMap: Map<number, string>;

  states: IState[];
  products: IProductTypeGroup[] = [];
  equipment: { id: number; name: string }[] = [];
  email = '';

  constructor(
    public dialogRef: MatDialogRef<InitProfileComponent>,
    private router: Router,
    private backend: BackendService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    productTypesSvc: ProductTypeService,
    equipmentSvc: EquipmentService,
    stateSvc: StatesService,
    authSvc: AuthService,
    private makerEquipmentSvc: MakerEquipmentService
  ) {
    this.firstFormGroup = this.fb.group(
      {
        isSupplier: [false],
        isRequestor: [false],
        isNeither: [false]
      },
      { validators: this.verifyRoles }
    );
    this.locationFormGroup = this.fb.group({
      companyName: null,
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      city: ['', Validators.required],
      state: ['AR', Validators.required],
      zipCode: ['', Validators.required],
      isSelfQuarantined: [false],
      hasCadSkills: [false]
    });
    authSvc.userProfile$.subscribe((profile) => {
      this.locationFormGroup.get('email').setValue(profile.email);
    });
    this.supplierFormGroup = this.fb.group({
      products: [[], this.verifyInventory],
      equipment: [[]]
    });
    this.requestorFormGroup = this.fb.group({
      secondCtrl: ['', Validators.required]
    });
    this.states = stateSvc.states;
    productTypesSvc.getProductHierarchy().subscribe((products) => {
      this.products = products;
      of(products)
        .pipe(
          flatMap((group) => group),
          flatMap((group) => group.products),
          toArray()
        )
        .subscribe((p) => {
          this.productMap = new Map(p.map((e) => [e.id, e.name] as [number, string]));
        });
    });
    equipmentSvc.lookup().subscribe((equipment) => {
      this.equipment = equipment;
    });
  }

  ngOnInit() {}

  handleProductChanges(event: MatSelectChange) {
    const currentValues = <AddInventoryDto[]>this.supplierFormGroup.get('products').value;
    const selection = <IProductEntry[]>event.value;
    const known = new Set<number>(currentValues.map((e) => e.productId));
    const incoming = new Map(selection.map((e) => [e.id, e] as [number, IProductEntry]));
    known.forEach((e) => {
      if (!incoming.has(e)) {
        const existing = currentValues.findIndex((p) => p.productId === event.value.id);
        currentValues.splice(existing, 1);
      }
    });
    incoming.forEach((value, key) => {
      if (!known.has(key)) {
        currentValues.push(
          new AddInventoryDto({
            productId: value.id
          })
        );
      }
    });
    this.supplierFormGroup.get('products').updateValueAndValidity();
  }

  handleEquipmentChanges(event: MatSelectChange) {
    this.supplierFormGroup.get('equipment').value.push(
      new MakerEquipmentDto({
        equipmentId: event.value.id,
        equipmentName: event.value.name
      })
    );
    event.source.writeValue(null);
    this.supplierFormGroup.get('equipment').updateValueAndValidity();
  }

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

  verifyRoles: ValidatorFn = (): ValidationErrors | null => {
    return this.firstFormGroup && !this.isSupplier && !this.isRequestor && !this.isNeither
      ? { lackingRoles: true }
      : null;
  };

  verifyInventory: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    const data = <AddInventoryDto[]>control.value;
    return data.length && data.some((e) => !(e.amount > 0)) ? { invalidAmount: true } : null;
  };

  submit() {
    zip(
      this.backend.saveProfile(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor,
          ...this.locationFormGroup.value
        })
      ),
      this.isSupplier ? this.backend.saveInventory(this.supplierFormGroup.get('products').value) : of(null),
      this.isSupplier ? this.makerEquipmentSvc.bulkSave(this.supplierFormGroup.get('equipment').value) : of(null)
    ).subscribe(
      () => {
        this.closeDialog();
        this.router.navigate(['/dashboard']);
      },
      () => {
        this.snackBar.open('Error', undefined, {
          duration: 3000
        });
      }
    );
  }
}
