import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {Meme} from '../../models/meme.model';
import {MemeForm} from '../../models/forms/meme.form';
import {Alias} from '../../models/alias.model';
import {MemesHttpService} from '../../services/memes-http.service';
import {AttachmentTypes} from '../../models/attachment-types.enum';
import {Attachment} from '../../models/attachment.model';
import {DialogMods} from '../../../models/dialog-mods.enum';

@Component({
  selector: 'app-create-edit-memes',
  templateUrl: './create-edit-memes.component.html',
  styleUrls: ['./create-edit-memes.component.scss'],
  providers: [MemesHttpService]
})
export class CreateEditMemesComponent implements OnInit, OnChanges {

  @Input() meme: Meme;

  @Input() dialogMode: DialogMods;


  @Output()
  success = new EventEmitter<any>();

  form: MemeForm;

  get AttachmentTypes() {
    return AttachmentTypes;
  }

  constructor(private memeService: MemesHttpService) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.meme) {
      this.form = new MemeForm();
      this.fillForm();
    }
  }

  ngOnInit() {
  }

  submit(): boolean {
    if (!this.form.valid) {
      console.log('form is bad');
      return false;
    }

    const oldAliases = this.form.aliases.value.filter(a => typeof (a) !== 'string');
    const newAliasStrings = this.form.aliases.value.filter(a => typeof (a) === 'string');
    const newAliases = [];
    for (const newAliasString of newAliasStrings) {
      newAliases.push(new Alias(newAliasString));
    }

    this.form.aliases.setValue(oldAliases.concat(newAliases));
    this.form.attachments.setValue([new Attachment(this.form.attachmentType.value, this.form.attachments.value)]);

    const memeObs = this.dialogMode === DialogMods.Edit ? this.memeService.updateMeme(this.form) : this.memeService.addMeme(this.form);
    memeObs.subscribe(() => {
        console.log('form is good');
        this.success.emit();
      },
      (error) => console.log(error));
  }

  private fillForm() {
    if (!this.meme) {
      return;
    }

    this.form.name.setValue(this.meme.name);
    this.form.id.setValue(this.meme.id);

    const firstAttachment = this.meme.attachments[0];
    this.form.attachments.setValue(firstAttachment.url);
    this.form.attachmentType.setValue(firstAttachment.type);

    this.form.aliases.setValue(this.meme.aliases);
  }
}
