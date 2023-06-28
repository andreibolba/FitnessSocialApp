import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Person } from 'src/model/person.model';


@Injectable({
  providedIn: 'root',
})
export class PersonDataService {
  private baseUrl = 'https://localhost:7191/api/';

  personAdded=new BehaviorSubject<Person | null>(null);

  constructor(private http: HttpClient) { }
  getPerson(username: string, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person>(this.baseUrl + 'people/' + username, {
      headers: headers,
    });
  }

  sendPictureForPerson(token: string, fd: FormData, personId: number) {
    const headers = { Authorization: 'Bearer ' + token, Accept: 'application/json' };
    return this.http.post(this.baseUrl + 'people/picture/add/' + personId, fd, { headers: headers });
  }


  getPersonById(id: number, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person>(this.baseUrl + 'people/' + id, {
      headers: headers,
    });
  }

  getPeople(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person[]>(this.baseUrl + 'people', {
      headers: headers,
    });
  }

  deletePerson(personId: number, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'people/delete/' + personId,
      {},
      {
        headers: headers,
      }
    );
  }

  addperson(person: Person, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Person>(
      this.baseUrl + 'account/register',
      {
        FirstName: person.firstName,
        LastName: person.lastName,
        Email: person.email,
        Username: person.username,
        Status: person.status,
        BirthDate: person.birthDate,
        Created: new Date(),
      },
      { headers: headers }
    );
  }

  editperson(person: Person, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Person>(
      this.baseUrl + 'people/update',
      {
        PersonId: person.personId,
        FirstName: person.firstName,
        LastName: person.lastName,
        Email: person.email,
        Username: person.username,
        Status: person.status,
        BirthDate: person.birthDate,
      },
      { headers: headers }
    );
  }

  sendEmail(email: string) {
    return this.http.post(this.baseUrl + 'people/forgot', {
      email: email
    });
  }

  isLinkValid(linkid: number) {
    return this.http.post(this.baseUrl + 'people/link', {
      linkid: linkid
    });
  }

  resetPassword(linkId: number, password: string) {
    return this.http.post(this.baseUrl + 'people/reset', {
      linkid: linkId,
      password: password,
    });
  }

  
}
