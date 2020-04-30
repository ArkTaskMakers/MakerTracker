import { Component, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { ProfileDto } from "autogen/ProfileDto";
import { BackendService } from "src/app/services/backend/backend.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { StatesService, IState } from "src/app/services/states.service";
import { ProductTypeService } from "src/app/services/backend/crud/productType.service";
import { IProductTypeGroup } from "src/app/ui-models/productTypeGroup";

@Component({
  selector: "app-init-profile",
  templateUrl: "./init-profile.component.html",
  styleUrls: ["./init-profile.component.scss"],
})
export class InitProfileComponent implements OnInit {
  isSupplier = false;
  isRequestor = false;
  isNeither = false;

  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  supplierFormGroup: FormGroup;
  requestorFormGroup: FormGroup;

  states: IState[];
  products: IProductTypeGroup[];

  constructor(
    public dialogRef: MatDialogRef<InitProfileComponent>,
    private backend: BackendService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    productTypesSvc: ProductTypeService,
    stateSvc: StatesService
  ) {
    this.states = stateSvc.states;
    productTypesSvc.getProductHierarchy().subscribe((products) => {
      this.products = products;
    });
  }

  ngOnInit() {
    this.secondFormGroup = this.fb.group({
      city: ["", Validators.required],
      state: ["AR", Validators.required],
      zipCode: ["", Validators.required],
    });
    this.supplierFormGroup = this.fb.group({
      products: [[]],
    });
    this.requestorFormGroup = this.fb.group({
      secondCtrl: ["", Validators.required],
    });
  }

  closeDialog() {
    if (this.isSupplier || this.isRequestor || this.isNeither) {
      this.dialogRef.close(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor,
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

  submit() {
    this.backend
      .saveProfile(
        new ProfileDto({
          isSupplier: this.isSupplier,
          isRequestor: this.isRequestor,
        })
      )
      .subscribe(
        () => this.closeDialog(),
        (err) => {
          this.snackBar.open("Error", undefined, {
            duration: 3000,
          });
        }
      );
  }
}
