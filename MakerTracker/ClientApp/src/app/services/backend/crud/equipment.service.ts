import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EquipmentDto } from 'autogen/EquipmentDto';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class EquipmentService extends GenericCrudService<EquipmentDto> {
  /**
   * Initializes a new instance of the EquipmentService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/Equipment', http);
  }
}
