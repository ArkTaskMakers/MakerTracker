import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { Product } from '../product';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  id: string;
  product: Product;
  feedback: any = {};
  src = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private _snackBar: MatSnackBar,
    private productService: ProductService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params
      .pipe(
        map((p) => p.id),
        switchMap((id) => {
          if (id === 'new') {
            return of(new Product());
          }
          return this.productService.findById(id);
        })
      )
      .subscribe(
        (product) => {
          this.product = product;
          this.feedback = {};
        },
        (err) => {
          this._snackBar.open('Error loading', null, {
            duration: 2000
          });
        }
      );
  }

  save() {
    this.productService.save(this.product).subscribe(
      (product) => {
        this.product = product;
        this._snackBar.open('Save was successful!', null, {
          duration: 2000
        });
        this.router.navigate(['/products']);
      },
      (err) => {
        this._snackBar.open('Error Saving', null, {
          duration: 2000
        });
      }
    );
  }

  cancel() {
    this.router.navigate(['/products']);
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.compressImage(reader.result, 64, 64).then(
          (compressed: string) => {
            this.src = compressed;
            this.product.imageUrl = compressed;
          },
          (err) => {
            this._snackBar.open('Invalid image', null, {
              duration: 2000
            });
          }
        );

        // need to run CD since file load runs outside of zone
        this.cd.markForCheck();
      };
    }
  }

  compressImage(src, newX, newY) {
    return new Promise((res, rej) => {
      const img = new Image();
      img.src = src;
      img.onload = () => {
        const elem = document.createElement('canvas');
        elem.width = newX;
        elem.height = newY;
        const ctx = elem.getContext('2d');
        ctx.drawImage(img, 0, 0, newX, newY);
        const data = ctx.canvas.toDataURL('image/jpeg', 0.75);
        res(data);
      };
      img.onerror = (error) => rej(error);
    });
  }
}
