import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductService } from './product.service';
import { PRODUCT_ROUTES } from './product.routes';
import { MaterialModule } from '../material/material.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    RouterModule.forChild(PRODUCT_ROUTES)
  ],
  declarations: [
    ProductListComponent,
    ProductEditComponent
  ],
  providers: [ProductService],
  exports: []
})
export class ProductModule { }
