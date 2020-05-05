import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MakerStockDto } from 'autogen/MakerStockDto';
import { Observable } from 'rxjs';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class MakerStockService extends GenericCrudService<MakerStockDto> {
  /**
   * Initializes a new instance of the ProductTypeService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/MakerStock', http);
  }

  list() {
    return this.query(this._baseUrl, {
      $orderBy: 'UpdatedOn'
    });
  }

  bulkSave(payload: MakerStockDto[]): Observable<any> {
    return this._http.post('api/MakerStock/bulk', payload);
  }
}
