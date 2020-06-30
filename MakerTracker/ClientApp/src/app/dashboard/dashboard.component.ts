import { ComponentType } from '@angular/cdk/overlay';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';
import { InventoryTransactionDto } from 'autogen/InventoryTransactionDto';
import { NeedDto } from 'autogen/NeedDto';
import { ProductDto } from 'autogen/ProductDto';
import { ProfileDto } from 'autogen/ProfileDto';
import { of } from 'rxjs';
import { flatMap, toArray } from 'rxjs/operators';
import { FormDialogConfig } from '../components/form-dialog/form-dialog-config.model';
import { FormDialogComponent } from '../components/form-dialog/form-dialog.component';
import { BackendService } from '../services/backend/backend.service';
import { NeedService } from '../services/backend/crud/need.service';
import { ProductTypeService } from '../services/backend/crud/productType.service';
import { IProductEntry, IProductTypeGroup } from '../ui-models/productTypeGroup';
import { DeliveryFormModel } from './dialogs/delivery-form.model';
import { InventoryFormModel } from './dialogs/inventory-form.model';
import { NeedFormModel } from './dialogs/need-form.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  isSupplier = false;
  isRequestor = false;
  inventorySummary: InventoryProductSummaryDto[];
  productMap: Map<number, IProductEntry>;
  products: IProductTypeGroup[];
  needs: NeedDto[];

  constructor(
    private backend: BackendService,
    private productTypesSvc: ProductTypeService,
    public dialog: MatDialog,
    private needSvc: NeedService
  ) {
    backend.getProfile().subscribe((profile: ProfileDto) => {
      this.isRequestor = profile.isRequestor;
      this.isSupplier = profile.isSupplier;
      this.refreshDashboard();
    });
    productTypesSvc.getProductHierarchy().subscribe((products) => {
      this.products = products;
      of(products)
        .pipe(
          flatMap((group) => group),
          flatMap((group) => group.products),
          toArray()
        )
        .subscribe((p) => {
          this.productMap = new Map(p.map((e) => [e.id, e] as [number, IProductEntry]));
        });
    });
  }

  ngOnInit(): void {}

  private refreshDashboard() {
    if (this.isSupplier) {
      this.refreshInventory();
    }
    if (this.isRequestor) {
      this.refreshNeeds();
    }
  }

  private refreshInventory() {
    this.backend.getInventorySummary().subscribe((res) => (this.inventorySummary = res));
  }

  private refreshNeeds() {
    this.needSvc.list().subscribe((res) => (this.needs = res));
  }

  private getProductField(need: NeedDto, field: string): string {
    const product = this.productMap.get(need.productId);
    return product && product[field];
  }

  openDialog(tmpl: ComponentType<unknown> | TemplateRef<unknown>, data: any, callback: (result: any) => void) {
    const dialogRef = this.dialog.open(tmpl, {
      data
    });

    dialogRef.afterClosed().subscribe((result) => callback(result));
  }

  openInventoryDialog(data: InventoryProductSummaryDto): void {
    this.openDialog(
      FormDialogComponent,
      new FormDialogConfig({
        title: data ? 'Edit Inventory' : 'Add Inventory',
        model: new InventoryFormModel(
          data
            ? new InventoryTransactionDto({
                product: new ProductDto({
                  id: data.productId,
                  name: data.productName,
                  imageUrl: data.productImageUrl
                }),
                amount: data.amount
              })
            : null,
          this.backend,
          this.productTypesSvc
        )
      }),
      () => this.refreshDashboard()
    );
  }

  openNeedDialog(data: NeedDto): void {
    this.openDialog(
      FormDialogComponent,
      new FormDialogConfig({
        title: data ? 'Edit Existing Need' : 'Create Need Request',
        model: new NeedFormModel(data, this.needSvc, this.products, this.productMap)
      }),
      () => this.refreshNeeds()
    );
  }

  fulfill(data: NeedDto): void {
    this.needSvc.fulfill(data).subscribe(() => this.refreshNeeds());
  }

  openDeliveryDialog(data: InventoryProductSummaryDto): void {
    this.openDialog(
      FormDialogComponent,
      new FormDialogConfig({
        title: 'Deliver Product',
        model: new DeliveryFormModel(data, this.backend, this.productMap, this.needSvc)
      }),
      () => this.refreshDashboard()
    );
  }
}
