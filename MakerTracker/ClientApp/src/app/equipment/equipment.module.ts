import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EquipmentListComponent } from './equipment-list/equipment-list.component';
import { EquipmentEditComponent } from './equipment-edit/equipment-edit.component';
import { EQUIPMENT_ROUTES } from './equipment.routes';
import { MaterialModule } from '../material/material.module';
import { EquipmentService } from '../services/backend/crud/equipment.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    RouterModule.forChild(EQUIPMENT_ROUTES)
  ],
  declarations: [
    EquipmentListComponent,
    EquipmentEditComponent
  ],
  providers: [EquipmentService],
  exports: []
})
export class EquipmentModule { }
