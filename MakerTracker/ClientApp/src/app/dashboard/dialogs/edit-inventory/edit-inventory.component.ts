import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EditInventoryDto } from 'autogen/EditInventoryDto';
import { ProductDto } from 'autogen/ProductDto';
import { BackendService } from 'src/app/services/backend/backend.service';

@Component({
  selector: 'app-edit-inventory',
  templateUrl: './edit-inventory.component.html',
  styleUrls: ['./edit-inventory.component.css']
})
export class EditInventoryComponent implements OnInit {
  product: ProductDto;

  constructor(
    private backend: BackendService,
    public dialogRef: MatDialogRef<EditInventoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditInventoryDto
  ) {}

  ngOnInit() {
    this.backend.getProduct(this.data.productId).subscribe((p) => {
      this.product = p;
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    this.backend.editInventory(this.data).subscribe((result) => {
      if (result) {
        this.dialogRef.close();
      } else {
        // error!
      }
    });
  }
}
