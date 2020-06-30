import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { AdminProfileDto } from 'autogen/AdminProfileDto';
import { UpdateProfileDto } from 'autogen/UpdateProfileDto';
import { BackendService } from 'src/app/services/backend/backend.service';
import { IState, StatesService } from 'src/app/services/states.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss']
})
export class UpdateProfileComponent implements OnInit {
  profileForm = this.fb.group({
    id: null,
    companyName: null,
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    city: [null, Validators.required],
    state: ['AR', Validators.required],
    bio: null,
    phone: null,
    email: [null, [Validators.required, Validators.email]],
    isSelfQuarantined: null,
    isDropOffPoint: null,
    zipCode: [null, Validators.compose([Validators.required, Validators.minLength(5), Validators.maxLength(5)])],
    isRequestor: false,
    isSupplier: false,
    hasCadSkills: false,
    adminNotes: null,
    hasOnboarded: false
  });

  states: IState[];
  loading: boolean;
  isAdminMode = false;

  constructor(
    private fb: FormBuilder,
    private backend: BackendService,
    private _snackBar: MatSnackBar,
    stateSvc: StatesService,
    private route: ActivatedRoute
  ) {
    this.states = stateSvc.states;
  }

  ngOnInit(): void {
    this.backend.getProfile(this.route.snapshot.paramMap.get('id')).subscribe((x) => this.patchValues(x));
    this.isAdminMode = !!this.route.snapshot.paramMap.get('id');
  }

  onSubmit() {
    if (!this.profileForm.valid) {
      this._snackBar.open('Please Validate your Profile', null, {
        duration: 2000
      });
      return;
    }

    this.loading = true;
    this.backend
      .saveProfile(this.profileForm.value as UpdateProfileDto, this.route.snapshot.paramMap.get('id'))
      .subscribe((x) => {
        this._snackBar.open('Your profile is Updated!', null, {
          duration: 2000
        });
        this.loading = false;
      });
  }

  patchValues(data: AdminProfileDto) {
    this.profileForm.patchValue({
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
      isSupplier: data.isSupplier,
      adminNotes: data.adminNotes,
      hasOnboarded: data.hasOnboarded
    });
  }
}
