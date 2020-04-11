import { Component, OnInit } from '@angular/core';
import { ProductFilter } from '../product-filter';
import { ProductService } from '../product.service';
import { Product } from '../product';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product',
  templateUrl: 'product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {

  filter = new ProductFilter();
  selectedProduct: Product;
  feedback: any = {};

  get productList(): Product[] {
    return this.productService.productList;
  }

  constructor(private productService: ProductService, private _snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.search();
  }

  search(): void {
    this.productService.load(this.filter);
  }

  select(selected: Product): void {
    this.selectedProduct = selected;
  }

  delete(product: Product): void {
    if (confirm('Are you sure?')) {
      this.productService.delete(product).subscribe(() => {
        this.search();
        this._snackBar.open('Delete was successful', null, {
          duration: 2000,
        });
      },
        err => {
          this._snackBar.open('Error Deleting', null, {
            duration: 2000,
          });
        }
      );
    }
  }
}
