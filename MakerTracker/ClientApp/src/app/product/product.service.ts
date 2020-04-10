import { Product } from './product';
import { ProductFilter } from './product-filter';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

const headers = new HttpHeaders().set('Accept', 'application/json');

@Injectable()
export class ProductService {
  productList: Product[] = [];
  api = '/api/products';

  constructor(
    private http: HttpClient, @Inject('BASE_URL') private baseUrl: string
  ) {
    this.api = baseUrl + 'api/products';
  }

  findById(id: string): Observable<Product> {
    const url = `${this.api}/${id}`;
    const params = { id: id };
    return this.http.get<Product>(url, { params, headers });
  }

  load(filter: ProductFilter): void {
    this.find(filter).subscribe(result => {
      this.productList = result;
    },
      err => {
        console.error('error loading', err);
      }
    );
  }

  find(filter: ProductFilter): Observable<Product[]> {
    const params = {
      'name': filter.name,
    };

    return this.http.get<Product[]>(this.api, { params, headers });
  }

  save(entity: Product): Observable<Product> {
    let params = new HttpParams();
    let url = '';
    if (entity.id) {
      url = `${this.api}/${entity.id.toString()}`;
      params = new HttpParams().set('ID', entity.id.toString());
      return this.http.put<Product>(url, entity, { headers, params });
    } else {
      url = `${this.api}`;
      return this.http.post<Product>(url, entity, { headers, params });
    }
  }

  delete(entity: Product): Observable<Product> {
    let params = new HttpParams();
    let url = '';
    if (entity.id) {
      url = `${this.api}/${entity.id.toString()}`;
      params = new HttpParams().set('ID', entity.id.toString());
      return this.http.delete<Product>(url, { headers, params });
    }
    return null;
  }
}

