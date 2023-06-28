import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { InternGroup } from 'src/model/interngroup.model';
import { Person } from 'src/model/person.model';


@Injectable({
  providedIn: 'root',
})
export class GroupDataService {
  private baseUrl = 'https://localhost:7191/api/';

  groupAdded = new BehaviorSubject<Group | null>(null);

  constructor(private http: HttpClient) { }

  getGroups(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Group[]>(this.baseUrl + 'group', { headers: headers });
  }

  getGroupById(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Object>(this.baseUrl + 'group/' + groupId, { headers: headers });
  }

  sendPictureForGroup(token: string, fd: FormData, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token, Accept: 'application/json' };
    return this.http.post(this.baseUrl + 'group/picture/add/' + groupId, fd, { headers: headers });
  }

  addGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Group>(
      this.baseUrl + 'group/add',
      {
        groupName: group.groupName,
        description: group.description,
        trainerId: group.trainerId,
      },
      { headers: headers }
    );
  }

  editGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Group>(
      this.baseUrl + 'group/update',
      {
        groupId: group.groupId,
        groupName: group.groupName,
        description: group.description,
        trainerId: group.trainerId,
      },
      { headers: headers }
    );
  }

  deleteGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'group/delete/' + groupId,
      {},
      {
        headers: headers,
      }
    );
  }

  getAllInternsInGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<InternGroup[]>(
      this.baseUrl + 'interngroup/interns/' + groupId,
      {
        headers: headers,
      }
    );
  }

  getAllPeopleInGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person[]>(
      this.baseUrl + 'group/participants/' + groupId,
      {
        headers: headers,
      }
    );
  }

  updateInternInGroup(token: string, ids: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };

    return this.http.post<Group>(
      this.baseUrl + 'interngroup/interns/update/' + groupId,
      {
        ids: ids,
      },
      {
        headers: headers,
      }
    );
  }
  
}
