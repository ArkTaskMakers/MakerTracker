import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
<<<<<<< HEAD
import { MatListModule } from '@angular/material/list';

=======
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
>>>>>>> 5ff60c5c01a5a2455815880ceda8132afad864c0


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatMenuModule,
    MatToolbarModule,
    MatTableModule,
    MatCardModule,
<<<<<<< HEAD
    MatListModule
=======
    MatSidenavModule,
    MatListModule,
>>>>>>> 5ff60c5c01a5a2455815880ceda8132afad864c0
  ],
  exports: [
    MatInputModule,
    MatButtonModule,
    MatMenuModule,
    MatToolbarModule,
    MatTableModule,
    MatCardModule,
<<<<<<< HEAD
    MatListModule
=======
    MatSidenavModule,
    MatListModule,
>>>>>>> 5ff60c5c01a5a2455815880ceda8132afad864c0
  ]
})
export class MaterialModule { }
