import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NeedDto } from 'autogen/NeedDto';
import { NeedLookupDto } from 'autogen/NeedLookupDto';
import { Observable } from 'rxjs';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class NeedService extends GenericCrudService<NeedDto> {
  /**
   * Initializes a new instance of the ProductTypeService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/Needs', http);
  }

  list() {
    return this.query(this._baseUrl, {
      $orderBy: 'DueDate desc,CreatedDate',
      $filter: 'Quantity gt 0 and FulfilledDate eq null'
    });
  }

  listOutstanding(productId: number): Observable<NeedLookupDto[]> {
    return <Observable<NeedLookupDto[]>>(<unknown>this.query(`${this._baseUrl}/products/${productId}`, {
      $orderBy: 'DueDate desc,ProfileDisplayName,NeedId'
    }));
  }

  bulkSave(payload: NeedDto[]): Observable<any> {
    return this._http.post(`${this._baseUrl}/bulk`, payload);
  }

  fulfill(data: NeedDto): Observable<any> {
    return this._http.post(`${this._baseUrl}/fulfill/${data.id}`, true);
  }
}
