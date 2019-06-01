import {Attachment} from './attachment.model';
import {Alias} from './alias.model';

export class Meme {
  id: number;
  name: string;
  attachments: Attachment[];
  aliases: Alias[];
  embededLink: any;
}
