import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { FeedbackComponent } from './feedback/feedback.component';
import { FormDialogComponent } from './form-dialog/form-dialog.component';

@NgModule({
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule],
  declarations: [FormDialogComponent, FeedbackComponent],
  exports: [FormDialogComponent, FeedbackComponent]
})
export class MakerComponentsModule {}
