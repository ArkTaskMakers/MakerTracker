import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductListComponent } from './product-list/product-list.component';
import { PRODUCT_ROUTES } from './product.routes';
import { ProductService } from './product.service';

@NgModule({
  imports: [CommonModule, FormsModule, MaterialModule, RouterModule.forChild(PRODUCT_ROUTES)],
  declarations: [ProductListComponent, ProductEditComponent],
  providers: [ProductService],
  exports: []
})
export class ProductModule {}
