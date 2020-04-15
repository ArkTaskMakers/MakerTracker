import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ProductService } from '../product.service';
import { Product } from '../product';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {

  id: string;
  product: Product;
  feedback: any = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private _snackBar: MatSnackBar,
    private productService: ProductService) {
  }

  ngOnInit() {
    this
      .route
      .params
      .pipe(
        map(p => p.id),
        switchMap(id => {
          if (id === 'new') { return of(new Product()); }
          return this.productService.findById(id);
        })
      )
      .subscribe(product => {
        this.product = product;
        this.feedback = {};
      },
        err => {
          this._snackBar.open('Error loading', null, {
            duration: 2000,
          });
        }
      );
  }

  save() {
    this.productService.save(this.product).subscribe(
      product => {
        this.product = product;
        this._snackBar.open('Save was successful!', null, {
          duration: 2000,
        });
        this.router.navigate(['/products']);
      },
      err => {
        this._snackBar.open('Error Saving', null, {
          duration: 2000,
        });
      }
    );
  }

  cancel() {
    this.router.navigate(['/products']);
  }
}
