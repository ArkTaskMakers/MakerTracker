import { Injectable } from '@angular/core';
import { FeedbackDto } from 'autogen/FeedbackDto';
import { FeedbackType } from 'autogen/FeedbackType';
import { FeedbackService } from 'src/app/services/backend/crud/feedback.service';
import { BaseLookupModel } from '../lookup-model';

@Injectable()
export class FeedbackRecordModel extends BaseLookupModel<FeedbackDto> {
  constructor(service: FeedbackService) {
    super({
      canAdd: false,
      canExport: true,
      canEdit: false,
      canDelete: true,
      lookupName: 'feedback',
      lookupDisplayName: 'Feedback',
      service: service,
      columns: [
        { headerName: 'User', field: 'submittedBy' },
        { headerName: 'Feedback Type', field: 'type', valueGetter: (p) => FeedbackType[p.data.type] },
        { headerName: 'Comment', field: 'comment' },
        { headerName: 'Created Date', field: 'createdDate' }
      ]
    });
  }
}
