import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AdminProfileDto } from 'autogen/AdminProfileDto';
import { NeedDto } from 'autogen/NeedDto';
import { ProfileDto } from 'autogen/ProfileDto';
import { UpdateProfileDto } from 'autogen/UpdateProfileDto';
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

  isBusy = false;
  today = new Date();
  firstFormGroup: FormGroup;
  locationFormGroup: FormGroup;
  supplierFormGroup: FormGroup;
  requestorFormGroup: FormGroup;
  productMap: Map<number, IProductEntry>;
  selectedProducts: IProductEntry[] = [];

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
    private needSvc: NeedService,
    private eqpSvc: MakerEquipmentService
  ) {
    this.firstFormGroup = this.fb.group(
      {
        isSupplier: [false],
        isRequestor: [false],
        isNeither: [false]
      },
      { validators: this.verifyRoles }
    );
    this.backend.getProfile().subscribe((x) => this.patchProfile(x));
    this.locationFormGroup = this.fb.group({
      companyName: null,
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      address: [null],
      address2: [null],
      city: ['', Validators.required],
      state: ['AR', Validators.required],
      zipCode: ['', Validators.required],
      isSelfQuarantined: [false],
      hasCadSkills: [false],
      isDropOffPoint: [false]
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
          this.productMap = new Map(p.map((e) => [e.id, e] as [number, IProductEntry]));
          this.patchInventory();
          this.patchNeeds();
        });
    });
    equipmentSvc.lookup().subscribe((equipment) => {
      this.equipment = equipment;
      this.patchEquipment();
    });
  }

  ngOnInit() {}

  patchInventory() {
    this.backend.getInventorySummary().subscribe((inv) => {
      const products: FormArray = <FormArray>this.supplierFormGroup.get('products');
      const factory = this.createInventory(this.fb);
      inv
        .filter((entry) => entry.amount > 0)
        .forEach((entry) => {
          const selected = this.productMap.get(entry.productId);
          products.push(factory(selected, entry.productId, entry.amount));
          this.selectedProducts.push(selected);
        });
    });
  }

  idComparer(o1, o2) {
    return (o1 && o1.id) === (o2 && o2.id);
  }

  patchEquipment() {
    this.eqpSvc.list().subscribe((eqp) => {
      const equipment: FormArray = <FormArray>this.supplierFormGroup.get('equipment');
      eqp.forEach((entry) => {
        equipment.push(
          this.fb.group({
            equipmentId: entry.equipmentId,
            equipmentName: entry.equipmentName,
            manufacturer: entry.manufacturer,
            modelNumber: entry.modelNumber
          })
        );
      });
    });
  }

  patchNeeds() {
    this.needSvc.list().subscribe((needs) => {
      const products: FormArray = <FormArray>this.requestorFormGroup.get('needs');
      needs.forEach((entry) => {
        products.push(this.createNeed(this.fb, this.productMap.get(entry.productId), entry));
      });
    });
  }

  createInventory(fb: FormBuilder): (value: IProductEntry, key: number, amount?: number) => FormGroup {
    return (value: IProductEntry, key: number, amount: number = null) =>
      this.fb.group({
        product: value,
        productId: value.id,
        amount: [amount, [Validators.required, Validators.min(1)]]
      });
  }

  createNeed(fb: FormBuilder, value: IProductEntry, need?: Partial<NeedDto>): FormGroup {
    return this.fb.group({
      id: (need && need.id) || 0,
      profileId: (need && need.profileId) || 0,
      createdDate: (need && need.createdDate) || new Date(),
      productId: value.id,
      quantity: [need && need.quantity, [Validators.required, Validators.min(1)]],
      dueDate: [(need && need.dueDate) || null],
      specialInstructions: (need && need.specialInstructions) || null
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

  handleNeedChanges(event: MatSelectChange) {
    (this.requestorFormGroup.get('needs') as FormArray).push(this.createNeed(this.fb, event.value));
    event.source.writeValue(null);
    this.requestorFormGroup.get('needs').updateValueAndValidity();
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
    this.isBusy = true;
    zip(
      this.backend.saveProfile(
        new UpdateProfileDto({
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
        this.isBusy = false;
        this.closeDialog();
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => this.router.navigate(['/dashboard']));
      },
      () => {
        this.isBusy = false;
        this.snackBar.open('Error', undefined, {
          duration: 3000
        });
      }
    );
  }

  saveData(data: any[], saveDelegate: (input: any[]) => Observable<any>, condition: boolean = true) {
    return condition && data && data.length ? saveDelegate(data) : of(null);
  }

  getProductName(id: number) {
    const product = this.productMap.get(id);
    return product && product.name;
  }

  hasProducts(group: IProductTypeGroup) {
    return group && group.products && group.products.some((p) => !p.isDeprecated);
  }

  patchProfile(data: AdminProfileDto) {
    this.locationFormGroup.patchValue({
      id: data.id,
      companyName: data.companyName,
      firstName: data.firstName,
      lastName: data.lastName,
      address: data.address,
      address2: data.address2,
      city: data.city,
      state: data.state || 'AR',
      zipCode: data.zipCode,
      bio: data.bio,
      phone: data.phone,
      email: data.email,
      hasCadSkills: data.hasCadSkills,
      isSelfQuarantined: data.isSelfQuarantined,
      isDropOffPoint: data.isDropOffPoint,
      isRequestor: data.isRequestor,
      isSupplier: data.isSupplier
    });
    this.isSupplier = data.isSupplier;
    this.isRequestor = data.isRequestor;
  }
}
