import {Component, OnInit, ViewChild} from '@angular/core';
import {MemesHttpService} from '../../services/memes-http.service';
import {Meme} from '../../models/meme.model';
import {EmbedVideoService} from 'ngx-embed-video/dist';
import {AttachmentTypes} from '../../models/attachment-types.enum';
import {CreateEditMemesComponent} from '../create-edit-memes/create-edit-memes.component';
import {DialogMods} from '../../../models/dialog-mods.enum';

@Component({
  selector: 'app-meme-list',
  templateUrl: './meme-list.component.html',
  styleUrls: ['./meme-list.component.scss']
})
export class MemeListComponent implements OnInit {

  memes: Meme[];
  selectedMeme: Meme;
  display = false;
  selectedDialogMode: DialogMods;

  @ViewChild(CreateEditMemesComponent) dialogView;

  private loading: boolean;

  constructor(public memeService: MemesHttpService, private embedService: EmbedVideoService) {
  }

  ngOnInit() {
    this.initMemes();
  }

  showCreateDialog() {
    this.selectedDialogMode = DialogMods.Create;
    this.selectedMeme = undefined;
    this.dialogView.fillForm();
    this.display = true;
  }

  showEditDialog(meme: Meme) {
    this.selectedDialogMode = DialogMods.Edit;
    this.selectedMeme = meme;
    this.display = true;
  }

  onSuccess() {
    this.initMemes();
    this.closeDialog();
  }


  onLoading(loading: boolean) {
    this.loading = loading;
  }

  closeDialog() {
    this.display = false;
  }

  submitForm() {
    const success = this.dialogView.submit();
    if (success) {
      this.closeDialog();
    }
  }

  private initMemes() {
    this.loading = true;
    this.memeService.getMemes().subscribe(memes => {
      this.memes = memes;
      this.memes.forEach(m => {
        // NOTE: Now we get only one attachment
        const attachment = m.attachments || m.attachments.length > 0 ? m.attachments[0] : undefined;
        switch (attachment.type) {
          case AttachmentTypes.Image:
            m.embededLink = `<img src="${attachment.url}" class="meme-image">`;
            break;
          case AttachmentTypes.Video:
            m.embededLink = this.embedService.embed(attachment.url, {
              attr: {width: 360, height: 240}
            });
            break;
        }
      });
      this.loading = false;
    }, (error => {
      console.log(error);
      this.loading = false;
    }));
  }
}
