import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public inventory: InventoryProductSummaryDto[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<InventoryProductSummaryDto[]>(baseUrl + 'api/inventory').subscribe(result => {
      this.inventory = result;
    }, error => console.error(error));
  }
}


 interface InventoryProductSummaryDto {
  productId: number;
  productName: string;
  productImageUrl: string;
  amount: number;
}
