import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BackendService } from 'src/app/services/backend/backend.service';
import { ProfileDto } from 'autogen/ProfileDto';
import { UpdateProfileDto } from 'autogen/UpdateProfileDto';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss']
})

export class UpdateProfileComponent implements OnInit {
  profileForm = this.fb.group({
    companyName: null,
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    address: [null],
    address2: null,
    city: [null, Validators.required],
    state: [null, Validators.required],
    bio: null,
    phone: null,
    email: [null, [Validators.required, Validators.email]],
    isSelfQuarantined: null,
    isDropOffPoint: null,
    zipCode: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(5)])
    ],
    isRequestor: false,
    isSupplier: false,
  });

  states = [
    { name: 'Alabama', abbreviation: 'AL' },
    { name: 'Alaska', abbreviation: 'AK' },
    { name: 'American Samoa', abbreviation: 'AS' },
    { name: 'Arizona', abbreviation: 'AZ' },
    { name: 'Arkansas', abbreviation: 'AR' },
    { name: 'California', abbreviation: 'CA' },
    { name: 'Colorado', abbreviation: 'CO' },
    { name: 'Connecticut', abbreviation: 'CT' },
    { name: 'Delaware', abbreviation: 'DE' },
    { name: 'District Of Columbia', abbreviation: 'DC' },
    { name: 'Federated States Of Micronesia', abbreviation: 'FM' },
    { name: 'Florida', abbreviation: 'FL' },
    { name: 'Georgia', abbreviation: 'GA' },
    { name: 'Guam', abbreviation: 'GU' },
    { name: 'Hawaii', abbreviation: 'HI' },
    { name: 'Idaho', abbreviation: 'ID' },
    { name: 'Illinois', abbreviation: 'IL' },
    { name: 'Indiana', abbreviation: 'IN' },
    { name: 'Iowa', abbreviation: 'IA' },
    { name: 'Kansas', abbreviation: 'KS' },
    { name: 'Kentucky', abbreviation: 'KY' },
    { name: 'Louisiana', abbreviation: 'LA' },
    { name: 'Maine', abbreviation: 'ME' },
    { name: 'Marshall Islands', abbreviation: 'MH' },
    { name: 'Maryland', abbreviation: 'MD' },
    { name: 'Massachusetts', abbreviation: 'MA' },
    { name: 'Michigan', abbreviation: 'MI' },
    { name: 'Minnesota', abbreviation: 'MN' },
    { name: 'Mississippi', abbreviation: 'MS' },
    { name: 'Missouri', abbreviation: 'MO' },
    { name: 'Montana', abbreviation: 'MT' },
    { name: 'Nebraska', abbreviation: 'NE' },
    { name: 'Nevada', abbreviation: 'NV' },
    { name: 'New Hampshire', abbreviation: 'NH' },
    { name: 'New Jersey', abbreviation: 'NJ' },
    { name: 'New Mexico', abbreviation: 'NM' },
    { name: 'New York', abbreviation: 'NY' },
    { name: 'North Carolina', abbreviation: 'NC' },
    { name: 'North Dakota', abbreviation: 'ND' },
    { name: 'Northern Mariana Islands', abbreviation: 'MP' },
    { name: 'Ohio', abbreviation: 'OH' },
    { name: 'Oklahoma', abbreviation: 'OK' },
    { name: 'Oregon', abbreviation: 'OR' },
    { name: 'Palau', abbreviation: 'PW' },
    { name: 'Pennsylvania', abbreviation: 'PA' },
    { name: 'Puerto Rico', abbreviation: 'PR' },
    { name: 'Rhode Island', abbreviation: 'RI' },
    { name: 'South Carolina', abbreviation: 'SC' },
    { name: 'South Dakota', abbreviation: 'SD' },
    { name: 'Tennessee', abbreviation: 'TN' },
    { name: 'Texas', abbreviation: 'TX' },
    { name: 'Utah', abbreviation: 'UT' },
    { name: 'Vermont', abbreviation: 'VT' },
    { name: 'Virgin Islands', abbreviation: 'VI' },
    { name: 'Virginia', abbreviation: 'VA' },
    { name: 'Washington', abbreviation: 'WA' },
    { name: 'West Virginia', abbreviation: 'WV' },
    { name: 'Wisconsin', abbreviation: 'WI' },
    { name: 'Wyoming', abbreviation: 'WY' }
  ];
  loading: boolean;

  constructor(private fb: FormBuilder, private backend: BackendService, private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.backend.getProfile().subscribe(x => this.patchValues(x));
  }

  onSubmit() {
    if (!this.profileForm.valid) {
      this._snackBar.open('Please Validate your Profile', null, {
        duration: 2000,
      });
      return;
    }

    this.loading = true;
    this.backend.saveProfile(this.profileForm.value as UpdateProfileDto).subscribe(x => {
      this._snackBar.open('Your profile is Updated!', null, {
        duration: 2000,
      });
      this.loading = false;
    });
  }

  patchValues(data: ProfileDto) {
    this.profileForm.patchValue({
      companyName: data.companyName,
      firstName: data.firstName,
      lastName: data.lastName,
      address: data.address,
      address2: data.address2,
      city: data.city,
      state: data.state,
      zipCode: data.zipCode,
      bio: data.bio,
      phone: data.zipCode,
      email: data.email,
      isSelfQuarantined: data.isSelfQuarantined,
      isDropOffPoint: data.isDropOffPoint,
      isRequestor: data.isRequestor,
      isSupplier: data.isSupplier,
    });
  }
}
