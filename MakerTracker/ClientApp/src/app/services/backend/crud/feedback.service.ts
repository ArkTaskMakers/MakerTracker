import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FeedbackDto } from 'autogen/FeedbackDto';
import { GenericCrudService } from './genericCrud.service';

/** Service used for interacting with the REST API for feedback */
@Injectable({
  providedIn: 'root'
})
export class FeedbackService extends GenericCrudService<FeedbackDto> {
  /**
   * Initializes a new instance of the FeedbackService class.
   * @param http The http client for hitting the REST API.
   */
  constructor(http: HttpClient) {
    super('/api/Feedback', http);
  }
}
