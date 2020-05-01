import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { LookupEditorComponent } from './edit/lookup-editor.component';
import { LookupListComponent } from './list/lookup-list.component';
import { LOOKUP_MODELS } from './lookup-model-provider.service';
import { LOOKUP_ROUTES } from './lookup.routes';
import { EquipmentModel } from './models/equipment.model';
import { ProductTypeModel } from './models/product-type.model';

@NgModule({
  imports: [CommonModule, FormsModule, MaterialModule, RouterModule.forChild(LOOKUP_ROUTES)],
  declarations: [LookupListComponent, LookupEditorComponent],
  providers: [
    { provide: LOOKUP_MODELS, useClass: ProductTypeModel, multi: true },
    { provide: LOOKUP_MODELS, useClass: EquipmentModel, multi: true }
  ],
  exports: []
})
export class LookupModule {}
