import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {MemeListComponent} from "./components/meme-list/meme-list.component";

const routes: Routes = [{
  path: 'memes', component: MemeListComponent,
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MemesRoutes {
}
