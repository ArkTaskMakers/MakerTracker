import { Component, OnInit, Inject } from '@angular/core';
import { ProductDto } from 'autogen/ProductDto';
import { BackendService } from 'src/app/services/backend/backend.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddInventoryComponent } from '../add-inventory/add-inventory.component';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { EditInventoryDto } from 'autogen/EditInventoryDto';

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
    @Inject(MAT_DIALOG_DATA) public data: EditInventoryDto) { }

  ngOnInit() {
    this.backend.getProduct(this.data.productId).subscribe(p => {
      this.product = p;
    });

  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    this.backend.editInventory(this.data).subscribe(result => {
      if (result) {
        this.dialogRef.close();
      } else {
        // error!
      }
    });
  }
}
