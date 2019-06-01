import {FormControl, FormGroup, Validators} from '@angular/forms';

export class MemeForm extends FormGroup {
  id = new FormControl(null);
  name = new FormControl(null, [Validators.required]);
  aliases = new FormControl(null, [Validators.required]);
  // NOTE: Will work only with one attachment
  attachmentType = new FormControl(null, [Validators.required]);
  attachments = new FormControl(null, [Validators.required]);

  constructor() {
    super({});
    this.addControl('id', this.id);
    this.addControl('name', this.name);
    this.addControl('aliases', this.aliases);
    this.addControl('attachments', this.attachments);
    this.addControl('attachmentType', this.attachmentType);
  }

}
