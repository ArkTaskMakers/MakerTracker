import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BackendService } from 'src/app/backend/backend.service';
import { ProductDto } from 'autogen/ProductDto';
import { AddInventoryDto } from 'autogen/AddInventoryDto';

@Component({
  selector: 'app-add-inventory',
  templateUrl: './add-inventory.component.html',
  styleUrls: ['./add-inventory.component.css']
})
export class AddInventoryComponent implements OnInit {
  products: ProductDto[];

  constructor(
    private backend: BackendService,
    public dialogRef: MatDialogRef<AddInventoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddInventoryDto) { }

  ngOnInit() {
    this.backend.getProductList().subscribe(p => {
      return this.products = p;
    });

  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    this.backend.saveInventory(this.data).subscribe(result => {
      if (result) {
        this.dialogRef.close();
      } else {
        // error!
      }
    });
  }
}
