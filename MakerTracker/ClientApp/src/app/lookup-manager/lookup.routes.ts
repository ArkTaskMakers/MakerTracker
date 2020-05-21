import { Routes } from '@angular/router';
import { AdminGuard } from '../guards/admin.guard';
import { LookupEditorComponent } from './edit/lookup-editor.component';
import { LookupListComponent } from './list/lookup-list.component';
import { RequestorListComponent } from './requestor-list/requestor-list.component';
import { SupplierListComponent } from './supplier-list/supplier-list.component';

export const LOOKUP_ROUTES: Routes = [
  {
    path: 'admin/suppliers',
    component: SupplierListComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/requestors',
    component: RequestorListComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/:model',
    component: LookupListComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/:model/:id',
    component: LookupEditorComponent,
    canActivate: [AdminGuard]
  }
];
