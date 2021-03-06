import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AdminProfileDto } from 'autogen/AdminProfileDto';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';
import { InventoryTransactionDto } from 'autogen/InventoryTransactionDto';
import { ProductDto } from 'autogen/ProductDto';
import { RequestorReportDto } from 'autogen/RequestorReportDto';
import { SupplierReportDto } from 'autogen/SupplierReportDto';
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

  saveInventory(data: InventoryTransactionDto | InventoryTransactionDto[]): Observable<InventoryTransactionDto> {
    let urlSuffix = '';
    if (Array.isArray(data)) {
      urlSuffix = '/bulk';
    }
    return this._http.post<InventoryTransactionDto>(`${this.baseUrl}api/inventory${urlSuffix}`, data);
  }

  editInventory(data: InventoryTransactionDto): Observable<InventoryTransactionDto> {
    return this._http.put<InventoryTransactionDto>(`${this.baseUrl}api/inventory/${data.product.id}`, data);
  }

  getProductList(): Observable<ProductDto[]> {
    return this._http.get<ProductDto[]>(this.baseUrl + 'api/products');
  }

  getProduct(id: number): Observable<ProductDto> {
    return this._http.get<ProductDto>(`${this.baseUrl}api/products/${id}`);
  }

  getProfile(id: string = ''): Observable<AdminProfileDto> {
    if (id) {
      return this._http.get<AdminProfileDto>(`${this.baseUrl}api/profiles/${id}`);
    }

    return this._http.get<AdminProfileDto>(`${this.baseUrl}api/profiles`);
  }

  saveProfile(request: UpdateProfileDto, id: string = ''): Observable<boolean> {
    if (id) {
      return this._http.put<boolean>(`${this.baseUrl}api/profiles/${id}`, request);
    }

    return this._http.put<boolean>(`${this.baseUrl}api/profiles`, request);
  }

  getAllRequestors(): Observable<RequestorReportDto[]> {
    return this._http.get<RequestorReportDto[]>(`${this.baseUrl}api/adminReports/requestors`);
  }

  getAllSuppliers(): Observable<SupplierReportDto[]> {
    return this._http.get<SupplierReportDto[]>(`${this.baseUrl}api/adminReports/suppliers`);
  }
}
