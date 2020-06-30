import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductDto } from 'autogen/ProductDto';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class ProductService extends GenericCrudService<ProductDto> {
  /**
   * Initializes a new instance of the ProductTypeService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/Products', http);
  }

  list() {
    return this.query(this._baseUrl, {
      $orderBy: 'isDeprecated,name'
    });
  }
}
