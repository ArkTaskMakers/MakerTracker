import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { ProductDto } from 'autogen/ProductDto';
import { BackendService } from 'src/app/services/backend/backend.service';

@Component({
  selector: 'app-add-inventory',
  templateUrl: './add-inventory.component.html',
  styleUrls: ['./add-inventory.component.css']
})
export class AddInventoryComponent implements OnInit {
  products: ProductDto[];
  public selectedProduct: ProductDto;

  constructor(
    private backend: BackendService,
    public dialogRef: MatDialogRef<AddInventoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddInventoryDto
  ) {}

  ngOnInit() {
    this.backend.getProductList().subscribe((p) => {
      return (this.products = p);
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    this.backend.saveInventory(this.data).subscribe((result) => {
      if (result) {
        this.dialogRef.close();
      } else {
        // error!
      }
    });
  }

  onProductChange(productId: number) {
    this.selectedProduct = this.products.filter((p) => p.id === productId)[0];
  }
}
