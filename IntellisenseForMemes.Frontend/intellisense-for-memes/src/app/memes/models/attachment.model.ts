import {AttachmentTypes} from "./attachment-types.enum";

export class Attachment {
  id: number;
  type: AttachmentTypes;
  url: string;
}
