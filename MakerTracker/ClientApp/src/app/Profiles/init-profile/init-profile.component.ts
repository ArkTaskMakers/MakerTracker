import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ProfileDto } from 'autogen/ProfileDto';
import { BackendService } from 'src/app/services/backend/backend.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-init-profile',
  templateUrl: './init-profile.component.html',
  styleUrls: ['./init-profile.component.scss']
})
export class InitProfileComponent implements OnInit {

  isSupplier = false;
  isRequestor = false;
  isNeither = false;

  constructor(public dialogRef: MatDialogRef<InitProfileComponent>,
    private backend: BackendService, private snackBar: MatSnackBar) { }

  closeDialog() {
    if(this.isSupplier || this.isRequestor || this.isNeither) {
      this.dialogRef.close(new ProfileDto({
        isSupplier: this.isSupplier,
        isRequestor: this.isRequestor
      }));
    }
  }

  refreshNeither() {
    if(this.isSupplier || this.isRequestor) {
      this.isNeither = false;
    }
  }

  refreshOthers() {
    if(this.isNeither) {
      this.isSupplier = false;
      this.isRequestor = false;
    }
  }

  ngOnInit(): void {
  }

  submit() {
    this.backend.saveProfile(new ProfileDto({
      isSupplier: this.isSupplier,
      isRequestor: this.isRequestor
    })).subscribe(() => this.closeDialog(), err => {
      this.snackBar.open('Error', undefined, {
        duration: 3000
      });
    });
  }
}
