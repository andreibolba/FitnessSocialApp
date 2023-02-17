import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  baseUrl = 'https://localhost:7191/api/';
  people:Person[]=[];
  constructor(private http: HttpClient) {}

  getPerson(username: string,token:string) {
    const headers = { Authorization: 'Bearer '+ token };
    return this.http.get<Person>(this.baseUrl + 'people/' + username,{headers: headers});
  }

  getPeople(token:string) {
    const headers = { Authorization: 'Bearer '+ token };
    return this.http.get<Person[]>(this.baseUrl + 'people',{headers: headers});
  }

  getPeopleF() {
    return this.http.get<Person[]>(this.baseUrl + 'people');
  }

  getGroups(token:string){
    const headers = { Authorization: 'Bearer '+ token };
    return this.http.get<Group[]>(this.baseUrl + 'group',{headers: headers});
  }

  sendEmail(email:string){
    return this.http.post(this.baseUrl+'people/forgot',{email: email, time: new Date()});
  }

  isLinkValid(linkid:number){
    return this.http.post(this.baseUrl+'people/link',{linkid: linkid, time: new Date()});
  }

  resetPassword(linkId:number,password:string){
    return this.http.post(this.baseUrl+'people/reset',{linkid: linkId, password: password});
  }

  private baseURL = 'https://jsonplaceholder.typicode.com/posts';
  getData() {
    return this.http.get(this.baseURL);
  }
}
