import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { Feedback } from 'src/model/feedback.model';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root',
})
export class GroupChatDataService {
  private baseUrl = 'https://localhost:7191/api/';
  private hubUrl = 'https://localhost:7191/hubs/';
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<GroupChatMessage[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  newGroupChatMessage = new BehaviorSubject<GroupChatMessage | null>(null);

  constructor(private http: HttpClient) { }

  createHubConnection(user: Person, groupId: number, token: string) {
    console.log(this.hubUrl);
    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl + 'groupmessage?group=' + groupId, {
      accessTokenFactory: () => token
    })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveGroupMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewGroupChatMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          this.messageThreadSource.next([...messages, message]);
          this.newGroupChatMessage.next(message);
        }
      });
    })

  }



  addGroupChat(token: string, nameOfGroup: string, descriptionOfGroup: string, adminId: number, ids: number[]) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChatMessage>(this.baseUrl + 'groupchat/add', {
      NameOfGroup: nameOfGroup,
      DescriptionOfGroup: descriptionOfGroup,
      AdminId: adminId,
      Ids: ids
    }, { headers: headers });
  }

  editGroupChat(token: string, group: GroupChat) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/edit', {
      GroupChatId: group.groupChatId,
      GroupChatName: group.groupChatName,
      GroupChatDescription: group.groupChatDescription,
      AdminId: group.adminId
    }, { headers: headers });
  }

  sendPictureForGroupChat(token: string, fd: FormData, groupChatId: number) {
    const headers = { Authorization: 'Bearer ' + token, Accept: 'application/json' };
    return this.http.post(this.baseUrl + 'groupchat/picture/add/' + groupChatId, fd, { headers: headers });
  }

  getAllGroupChatsLastMessages(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<GroupChatMessage[]>(this.baseUrl + 'groupchat/' + personId, { headers: headers });
  }

  getAllMessagesForGroup(token: string, geroupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<GroupChatMessage[]>(this.baseUrl + 'groupchat/messages/' + geroupId, { headers: headers });
  }

  async sendMessageToGroupChat(token: string, personId: number, groupId: number, message: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.hubConnection?.invoke('AddGroupChatMessage', {
      PersonId: personId,
      GroupChatId: groupId,
      Message: message
    }).catch(error=>console.log(error));
  }

  deleteGroupChat(token: string, groupChatId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/delete/' + groupChatId, {}, { headers: headers });
  }

  removeMemberFromGroupChat(token: string, groupChatId: number, memberId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/deleteperson/' + groupChatId + '/' + memberId, {}, { headers: headers });
  }

  makeAdminGroupChat(token: string, groupChatId: number, memberId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/makeadmin/' + groupChatId + '/' + memberId, {}, { headers: headers });
  }

  updateMembersGroupChat(token: string, groupChatId: number, ids: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChat>(this.baseUrl + 'groupchat/update/members/' + groupChatId, { ids: ids }, { headers: headers });
  }
  
}
