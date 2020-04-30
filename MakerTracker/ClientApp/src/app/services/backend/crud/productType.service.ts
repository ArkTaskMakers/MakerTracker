import { GenericCrudService } from "./genericCrud.service";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { EquipmentDto } from "autogen/EquipmentDto";
import { Observable } from "rxjs";
import { IProductTypeGroup } from "src/app/ui-models/productTypeGroup";

/** Service used for interacting with the REST API for equipment */
@Injectable({
  providedIn: 'root'
})
export class ProductTypeService extends GenericCrudService<EquipmentDto> {

  /**
    * Initializes a new instance of the EquipmentService class.
    * @param http The http client for hitting the REST API.
    */
  constructor(http: HttpClient) {
    super('/api/Equipment', http);
  }

  getProductHierarchy(): Observable<IProductTypeGroup[]> {
    return this._http.get<IProductTypeGroup[]>('api/ProductTypes/Query?$expand=Products($filter=IsDeprecated%20eq%20false;$select=Id,Name,ImageUrl)&orderBy=SortOrder&$select=Id,Name');
  }
}
