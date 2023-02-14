import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  baseUrl = 'https://localhost:7191/api/';
  constructor(private http: HttpClient) {}

  getPerson(username: string,token:string) {
    const headers = { Authorization: 'Bearer '+ token };
    return this.http.get<Person>(this.baseUrl + 'people/' + username,{headers: headers});
  }
}
