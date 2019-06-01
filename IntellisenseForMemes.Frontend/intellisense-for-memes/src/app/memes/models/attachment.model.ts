import {AttachmentTypes} from './attachment-types.enum';

export class Attachment {
  id: number;
  type: AttachmentTypes;
  url: string;

  constructor(type: number, url: string) {
    this.type = type;
    this.url = url;
  }
}
