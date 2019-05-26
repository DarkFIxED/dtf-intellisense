import { Component, OnInit } from '@angular/core';
import {MemesHttpService} from "../../services/memes-http.service";
import {Meme} from "../../models/meme.model";

@Component({
  selector: 'app-meme-list',
  templateUrl: './meme-list.component.html',
  styleUrls: ['./meme-list.component.scss']
})
export class MemeListComponent implements OnInit {

  memes: Meme[];

  constructor(public memeService: MemesHttpService) { }

  ngOnInit() {
    this.memeService.getMemes().subscribe(memes => {
      this.memes = memes;
    });
  }
}
