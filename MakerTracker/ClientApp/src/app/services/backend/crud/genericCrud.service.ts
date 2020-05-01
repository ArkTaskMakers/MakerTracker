import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

/** A generic CRUD service for interacting with the REST API with less boilerplate */
export class GenericCrudService<T> {
  /**
   * Initializes a new instance of the GenericCrudService class.
   * @param _baseUrl The base url to hit for the controller
   * @param _http The http client for interacting with the REST API
   */
  constructor(protected _baseUrl: string, protected _http: HttpClient) {}

  /** Lists all entries */
  list(): Observable<T[]> {
    return this._http.get<T[]>(`${this._baseUrl}`);
  }

  /** Queries a specific url for entries */
  query(url: string, params: { [string: string]: string | string[] }): Observable<T[]> {
    return this._http.get<T[]>(url, {
      params
    });
  }

  /** Gets a specific entry */
  get(id: number): Observable<T> {
    return this._http.get<T>(`${this._baseUrl}/${id}`);
  }

  /** Updates an entry */
  update(id: number, payload: T): Observable<T> {
    return this._http.put<T>(`${this._baseUrl}/${id}`, payload);
  }

  /** Creates an entry */
  create(payload: T): Observable<T> {
    return this._http.post<T>(`${this._baseUrl}`, payload);
  }

  /** Deletes an entry - Note: the rename is to avoid issue with reserved language names. */
  destroy(id: number): Observable<T> {
    return this._http.delete<T>(`${this._baseUrl}/${id}`);
  }
}
