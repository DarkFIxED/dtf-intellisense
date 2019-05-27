import {Component, OnInit} from '@angular/core';
import {MemesHttpService} from '../../services/memes-http.service';
import {Meme} from '../../models/meme.model';
import {EmbedVideoService} from 'ngx-embed-video/dist';
import {AttachmentTypes} from '../../models/attachment-types.enum';

@Component({
  selector: 'app-meme-list',
  templateUrl: './meme-list.component.html',
  styleUrls: ['./meme-list.component.scss']
})
export class MemeListComponent implements OnInit {

  memes: Meme[];

  constructor(public memeService: MemesHttpService, private embedService: EmbedVideoService) {
  }

  ngOnInit() {
    this.memeService.getMemes().subscribe(memes => {
      this.memes = memes;
      this.memes.forEach(m => {
        // NOTE: Now we get only one attachment
        const attachment = m.attachments || m.attachments.length > 0 ? m.attachments[0] : undefined;
        switch (attachment.type) {
          case AttachmentTypes.Image:
            m.embededLink = this.embedService.embed_image(attachment.url, {
              attr: { width: 480, height: 360 }
            });
            break;
          case AttachmentTypes.Video:
            m.embededLink = this.embedService.embed(attachment.url, {
              attr: { width: 360, height: 240 }
            });
            break;
        }
      });
    });
  }
}
