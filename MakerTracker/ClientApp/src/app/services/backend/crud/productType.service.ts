import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductTypeDto } from 'autogen/ProductTypeDto';
import { Observable } from 'rxjs';
import { IProductTypeGroup } from 'src/app/ui-models/productTypeGroup';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class ProductTypeService extends GenericCrudService<ProductTypeDto> {
  /**
   * Initializes a new instance of the ProductTypeService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/ProductTypes', http);
  }

  list() {
    return this.query(this._baseUrl, {
      $orderBy: 'SortOrder'
    });
  }

  lookup() {
    return this.query(this._baseUrl, {
      $orderBy: 'SortOrder',
      $select: 'Id,Name'
    });
  }

  getProductHierarchy(): Observable<IProductTypeGroup[]> {
    return this.query('api/ProductTypes/Query', {
      $expand: 'Products($select=Id,Name,ImageUrl,IsDeprecated)',
      $orderBy: 'SortOrder',
      $select: 'Id,Name'
    });
  }
}
