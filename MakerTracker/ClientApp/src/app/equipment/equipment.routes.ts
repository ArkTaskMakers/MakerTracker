import { Routes } from '@angular/router';
import { EquipmentListComponent } from './equipment-list/equipment-list.component';
import { EquipmentEditComponent } from './equipment-edit/equipment-edit.component';

export const EQUIPMENT_ROUTES: Routes = [
  {
    path: 'equipment',
    component: EquipmentListComponent
  },
  {
    path: 'equipment/:id',
    component: EquipmentEditComponent
  }
];
