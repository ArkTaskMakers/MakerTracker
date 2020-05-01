import { Routes } from '@angular/router';
import { LookupEditorComponent } from './edit/lookup-editor.component';
import { LookupListComponent } from './list/lookup-list.component';

export const LOOKUP_ROUTES: Routes = [
  {
    path: 'admin/:model',
    component: LookupListComponent
  },
  {
    path: 'admin/:model/:id',
    component: LookupEditorComponent
  }
];
