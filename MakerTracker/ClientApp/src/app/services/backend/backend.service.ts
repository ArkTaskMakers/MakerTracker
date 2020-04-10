import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';
import { Observable } from 'rxjs';
import { ProductDto } from 'autogen/ProductDto';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { tap } from 'rxjs/operators';
import { EditInventoryDto } from 'autogen/EditInventoryDto';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor(
    private _http: HttpClient, @Inject('BASE_URL') private baseUrl: string
  ) { }


  getInventorySummary(): Observable<InventoryProductSummaryDto[]> {
    return this._http.get<InventoryProductSummaryDto[]>(this.baseUrl + 'api/inventory');
  }

  saveInventory(data: AddInventoryDto): Observable<boolean> {
    return this._http.post<boolean>(this.baseUrl + 'api/inventory', data);
  }

  editInventory(data: EditInventoryDto): Observable<boolean> {
    return this._http.put<boolean>(`${this.baseUrl}api/inventory/${data.productId}`, data);
  }

  getProductList(): Observable<ProductDto[]> {
    return this._http.get<ProductDto[]>(this.baseUrl + 'api/products');
  }

  getProduct(id: number): Observable<ProductDto> {
    return this._http.get<ProductDto>(`${this.baseUrl}api/products/${id}`);
  }
}
