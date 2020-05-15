import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ProfileDto } from 'autogen/ProfileDto';
import { Observable, of, zip } from 'rxjs';
import { flatMap, toArray } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BackendService } from 'src/app/services/backend/backend.service';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';
import { MakerEquipmentService } from 'src/app/services/backend/crud/makerEquipment.service';
import { NeedService } from 'src/app/services/backend/crud/need.service';
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

  today = new Date();
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
    public fb: FormBuilder,
    productTypesSvc: ProductTypeService,
    equipmentSvc: EquipmentService,
    stateSvc: StatesService,
    authSvc: AuthService,
    private makerEquipmentSvc: MakerEquipmentService,
    private needSvc: NeedService
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
      products: this.fb.array([]),
      equipment: this.fb.array([])
    });
    this.requestorFormGroup = this.fb.group({
      needs: this.fb.array([])
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

  createInventory(fb: FormBuilder): (value: IProductEntry, key: number) => FormGroup {
    return (value: IProductEntry, key: number) =>
      this.fb.group({
        productId: value.id,
        amount: [null, [Validators.required, Validators.min(1)]]
      });
  }

  createNeed(fb: FormBuilder): (value: IProductEntry, key: number) => FormGroup {
    return (value: IProductEntry, key: number) =>
      this.fb.group({
        productId: value.id,
        quantity: [null, [Validators.required, Validators.min(1)]],
        dueDate: [null],
        specialInstructions: null
      });
  }

  handleProductChanges(
    event: MatSelectChange,
    currentValues: FormArray,
    factory: (value: IProductEntry, key: number) => FormGroup
  ) {
    const selection = <IProductEntry[]>event.value;
    const known = new Set<number>(currentValues.controls.map((e) => e.get('productId').value));
    const incoming = new Map(selection.map((e) => [e.id, e] as [number, IProductEntry]));
    known.forEach((e) => {
      if (!incoming.has(e)) {
        const existing = currentValues.controls.findIndex((p) => p.get('productId').value === event.value.id);
        currentValues.controls.splice(existing, 1);
      }
    });
    incoming.forEach((value, key) => {
      if (!known.has(key)) {
        currentValues.push(factory(value, key));
      }
    });
    currentValues.updateValueAndValidity();
  }

  handleEquipmentChanges(event: MatSelectChange) {
    (this.supplierFormGroup.get('equipment') as FormArray).push(
      this.fb.group({
        equipmentId: event.value.id,
        equipmentName: event.value.name,
        manufacturer: null,
        modelNumber: null
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

  submit() {
    zip(
      this.backend.saveProfile(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor,
          ...this.locationFormGroup.value
        })
      ),
      this.saveData(
        this.supplierFormGroup.get('products').value,
        (data) => this.backend.saveInventory(data),
        this.isSupplier
      ),
      this.saveData(
        this.supplierFormGroup.get('equipment').value,
        (data) => this.makerEquipmentSvc.bulkSave(data),
        this.isSupplier
      ),
      this.saveData(this.requestorFormGroup.get('needs').value, (data) => this.needSvc.bulkSave(data), this.isRequestor)
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

  saveData(data: any[], saveDelegate: (input: any[]) => Observable<any>, condition: boolean = true) {
    return condition && data && data.length ? saveDelegate(data) : of(null);
  }
}
