import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { FeedbackService } from 'src/app/services/backend/crud/feedback.service';
import { FormDialogConfig } from '../form-dialog/form-dialog-config.model';
import { FormDialogComponent } from '../form-dialog/form-dialog.component';
import { FeedbackFormModel } from './feedback-form.model';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.scss']
})
export class FeedbackComponent implements OnInit {
  constructor(
    public dialog: MatDialog,
    protected snackbar: MatSnackBar,
    protected service: FeedbackService,
    protected router: Router
  ) {}

  ngOnInit(): void {}

  showForm() {
    const dialogRef = this.dialog.open(FormDialogComponent, {
      disableClose: true,
      data: new FormDialogConfig({
        title: 'Feedback',
        model: new FeedbackFormModel(this.service, this.router)
      })
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.snackbar.open('Thank you for your feedback.', null, { duration: 4000 });
      }
    });
  }
}
