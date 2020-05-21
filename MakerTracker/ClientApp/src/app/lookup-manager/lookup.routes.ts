import { Routes } from '@angular/router';
import { AdminGuard } from '../guards/admin.guard';
import { LookupEditorComponent } from './edit/lookup-editor.component';
import { LookupListComponent } from './list/lookup-list.component';

export const LOOKUP_ROUTES: Routes = [
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
