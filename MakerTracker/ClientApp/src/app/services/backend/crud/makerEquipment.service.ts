import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MakerEquipmentDto } from 'autogen/MakerEquipmentDto';
import { Observable } from 'rxjs';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class MakerEquipmentService extends GenericCrudService<MakerEquipmentDto> {
  /**
   * Initializes a new instance of the ProductTypeService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/MakerEquipment', http);
  }

  list() {
    return this.query(this._baseUrl, {
      $orderBy: 'EquipmentId,Manufacturer,ModelNumber,Id'
    });
  }

  bulkSave(payload: MakerEquipmentDto[]): Observable<any> {
    return this._http.post('api/MakerEquipment/bulk', payload);
  }
}
