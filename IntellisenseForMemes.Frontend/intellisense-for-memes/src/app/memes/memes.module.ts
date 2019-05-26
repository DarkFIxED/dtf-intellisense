import { NgModule } from '@angular/core';
import { MemeListComponent } from './components/meme-list/meme-list.component';
import {MemesRoutes} from "./memes.routes";
import {CommonModule} from "@angular/common";

@NgModule({
  declarations: [MemeListComponent],
  imports: [
    CommonModule,
    MemesRoutes
  ]
})
export class MemesModule { }
