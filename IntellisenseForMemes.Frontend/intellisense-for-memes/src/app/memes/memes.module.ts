import { NgModule } from '@angular/core';
import { MemeListComponent } from './components/meme-list/meme-list.component';
import {MemesRoutes} from "./memes.routes";
import {CommonModule} from "@angular/common";
import {DataViewModule} from "primeng/dataview";
import {DropdownModule} from "primeng/primeng";
import {EmbedVideo} from 'ngx-embed-video/dist';

@NgModule({
  declarations: [MemeListComponent],
  imports: [
    CommonModule,
    DropdownModule,
    DataViewModule,
    MemesRoutes,
    EmbedVideo.forRoot()
  ]
})
export class MemesModule { }
