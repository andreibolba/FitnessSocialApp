import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Meeting } from 'src/model/meeting.model';
import { ObjectGroup } from 'src/model/objectgroup.model';
import { ObjectIntern } from 'src/model/objectintern.model';


@Injectable({
  providedIn: 'root',
})
export class MeetingDataService {
  private baseUrl = 'https://localhost:7191/api/';

  meetignAdded = new BehaviorSubject<Meeting | null>(null);

  constructor(private http: HttpClient) { }

  addMeeting(token: string, meeting: Meeting) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Meeting>(
      this.baseUrl + 'meeting/add',
      {
        MeetingName: meeting.meetingName,
        MeetingLink: meeting.meetingLink,
        TrainerId: meeting.traierId,
        MeetingStartTime: meeting.meetingStartTime,
        MeetingFinishTime: meeting.meetingFinishTime,
        InterndIds: meeting.internIds,
        GroupIds: meeting.groupIds
      },
      {
        headers: headers,
      }
    );
  }

  updateMeeting(token: string, meeting: Meeting) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Meeting>(
      this.baseUrl + 'meeting/update',
      {
        MeetingId: meeting.meetingId,
        MeetingName: meeting.meetingName,
        MeetingLink: meeting.meetingLink,
        TrainerId: meeting.traierId,
        MeetingStartTime: meeting.meetingStartTime,
        MeetingFinishTime: meeting.meetingFinishTime,
        InterndIds: meeting.internIds,
        GroupIds: meeting.groupIds
      },
      {
        headers: headers,
      }
    );
  }

  deleteMeeting(token: string, meetingId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'meeting/delete/' + meetingId, {
      headers: headers,
    });
  }

  getAllMeetingsForPerson(token: string, id: number, status: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Meeting[]>(this.baseUrl + 'meeting/' + id + '/' + status, {
      headers: headers,
    });
  }

  getANumberOfMeetingsForPerson(token: string, id: number, status: string, count: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Meeting[]>(this.baseUrl + 'meeting/' + id + '/' + status + '/' + count, {
      headers: headers,
    });
  }

  getAllInternsInMeeting(token: string, meetingId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectIntern[]>(
      this.baseUrl + 'meeting/all/' + meetingId + '/interns',
      {
        headers: headers,
      }
    );
  }

  getAllGroupsInMeeting(token: string, meetingId: number, trainerId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectGroup[]>(
      this.baseUrl + 'meeting/all/' + meetingId + '/groups/' + trainerId,
      {
        headers: headers,
      }
    );
  }

}
