import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ApiResponse} from '../../models/api-response.model';
import {Meme} from '../models/meme.model';
import {environment} from '../../environments/environment';
import {map} from 'rxjs/operators';
import {MemeForm} from '../models/forms/meme.form';

@Injectable({
  providedIn: 'root'
})
export class MemesHttpService {
  private headers = new HttpHeaders({'Content-Type': 'application/json'});

  constructor(private http: HttpClient) {
  }

  public getMemes(): Observable<Meme[]> {
    return this.http.get<ApiResponse<Meme[]>>(environment.apiUrl + `memes`, {headers: this.headers}).pipe(
      map(r => ApiResponse.extractData(r))
    );
  }

  public updateMeme(form: MemeForm): Observable<Meme[]> {
    return this.http.put<ApiResponse<any>>(environment.apiUrl + `memes`, form.value, {headers: this.headers}).pipe(
      map(r => ApiResponse.extractData(r))
    );
  }

  public addMeme(form: MemeForm): Observable<Meme[]> {
    return this.http.post<ApiResponse<any>>(environment.apiUrl + `memes`, form.value, {headers: this.headers}).pipe(
      map(r => ApiResponse.extractData(r))
    );
  }
}
