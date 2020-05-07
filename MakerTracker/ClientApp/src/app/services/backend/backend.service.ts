import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { EditInventoryDto } from 'autogen/EditInventoryDto';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';
import { ProductDto } from 'autogen/ProductDto';
import { ProfileDto } from 'autogen/ProfileDto';
import { UpdateProfileDto } from 'autogen/UpdateProfileDto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackendService {
  constructor(private _http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  getInventorySummary(): Observable<InventoryProductSummaryDto[]> {
    return this._http.get<InventoryProductSummaryDto[]>(this.baseUrl + 'api/inventory');
  }

  saveInventory(data: AddInventoryDto | AddInventoryDto[]): Observable<boolean> {
    let urlSuffix = '';
    if (Array.isArray(data)) {
      urlSuffix = '/bulk';
    }
    return this._http.post<boolean>(`${this.baseUrl}api/inventory${urlSuffix}`, data);
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

  getProfile(): Observable<ProfileDto> {
    return this._http.get<ProfileDto>(`${this.baseUrl}api/profiles`);
  }

  saveProfile(request: UpdateProfileDto): Observable<boolean> {
    return this._http.put<boolean>(`${this.baseUrl}api/profiles`, request);
  }
}
