import {NgModule} from '@angular/core';
import {MemeListComponent} from './components/meme-list/meme-list.component';
import {MemesRoutes} from './memes.routes';
import {CommonModule} from '@angular/common';
import {DataViewModule} from 'primeng/dataview';
import {ChipsModule, DropdownModule, InputTextModule, ProgressSpinnerModule, RadioButtonModule} from 'primeng/primeng';
import {EmbedVideo} from 'ngx-embed-video/dist';
import {DialogModule} from 'primeng/dialog';
import {ButtonModule} from 'primeng/button';
import {CreateEditMemesComponent} from './components/create-edit-memes/create-edit-memes.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

@NgModule({
  declarations: [MemeListComponent, CreateEditMemesComponent],
  imports: [
    CommonModule,
    DropdownModule,
    InputTextModule,
    ProgressSpinnerModule,
    ReactiveFormsModule,
    FormsModule,
    ChipsModule,
    RadioButtonModule,
    DataViewModule,
    ButtonModule,
    MemesRoutes,
    DialogModule,
    EmbedVideo.forRoot()
  ]
})
export class MemesModule {
}
