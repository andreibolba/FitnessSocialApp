import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root',
})
export class PeopleService {
  baseUrl = 'https://localhost:7191/api/';
  constructor(private http: HttpClient) {}
  public loggedPerson = new Person();

  getPerson(username: string) {
    const headers = { Authorization: 'Bearer my-token' };
    return this.http.get<Person>(this.baseUrl + 'people/' + username);
  }
}
