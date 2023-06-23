import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  baseUrl = 'https://localhost:7191/api/';
  constructor(private http: HttpClient) { }

  log(model:any){
    return this.http.post(this.baseUrl + 'logging/log', model);
  }
}
