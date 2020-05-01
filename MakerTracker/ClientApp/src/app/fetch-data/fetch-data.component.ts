import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public displayedColumns: string[] = ['productId', 'productName', 'productImageUrl', 'amount'];
  public inventory: InventoryProductSummaryDto[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<InventoryProductSummaryDto[]>(baseUrl + 'api/inventory').subscribe(
      (result) => {
        this.inventory = result;
      },
      (error) => console.error(error)
    );
  }
}

interface InventoryProductSummaryDto {
  productId: number;
  productName: string;
  productImageUrl: string;
  amount: number;
}
