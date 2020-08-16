import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FeedbackDto } from 'autogen/FeedbackDto';
import { FeedbackType } from 'autogen/FeedbackType';
import { Observable, of } from 'rxjs';
import {
  FormDialogField,
  FormDialogModel,
  FormDialogSelectInputOptions,
  FormDialogTextAreaInputOptions,
  IFormDialogField
} from 'src/app/components/form-dialog/form-dialog-config.model';
import { FeedbackService } from 'src/app/services/backend/crud/feedback.service';

export class FeedbackFormModel extends FormDialogModel<FeedbackDto> {
  isEditMode = false;
  feedbackTypes = FeedbackType;
  form: FormGroup;

  formFields: IFormDialogField[] = [
    new FormDialogField({
      field: 'type',
      fieldType: 'select',
      isHidden: () => this.isEditMode,
      placeholder: 'Feedback Type',
      required: true,
      options: new FormDialogSelectInputOptions({
        fieldOptions: of(
          Object.keys(this.feedbackTypes)
            .filter((e) => typeof this.feedbackTypes[e] !== 'string')
            .map((e) => FeedbackType[e])
        ),
        getOptionDisplay: (type: FeedbackType) =>
          type === FeedbackType.MissingProduct ? 'Missing Product' : FeedbackType[type]
      })
    }),
    new FormDialogField({
      fieldType: 'description',
      label: this.getCommentLabel.bind(this)
    }),
    new FormDialogField({
      field: 'comment',
      fieldType: 'textarea',
      label: 'Your Message',
      options: new FormDialogTextAreaInputOptions({
        rows: 8
      })
    })
  ];
  constructor(protected service: FeedbackService, protected router: Router) {
    super(null, null);
  }

  buildForm(fb: FormBuilder) {
    this.form = fb.group({
      type: [FeedbackType.Comment, [Validators.required]],
      comment: ['', [Validators.required]]
    });
    return this.form;
  }

  onSubmit(data: FeedbackDto): Observable<FeedbackDto> {
    data.url = window.location.href;
    return this.service.create(data);
  }

  getCommentLabel(): string {
    const currentType = this.form && this.form.get('type').value;
    switch (currentType) {
      case FeedbackType.Problem:
        return 'Please list the issue. You can describe what you did and what you expected, and what happened instead.';
      case FeedbackType.Question:
        return 'What questions do you have?';
      case FeedbackType.Comment:
        return "Let us know what's on your mind!";
      case FeedbackType.MissingProduct:
        return "If you can make or need a product that isn't in the list, please put in your message: <ul><li>The exact product name</li><li>a product description - including the designated audience</li><li>the materials</li><li>a hyperlink showing an example of the product on the internet.</li></ul>";
      default:
        return 'Comment';
    }
  }
}
