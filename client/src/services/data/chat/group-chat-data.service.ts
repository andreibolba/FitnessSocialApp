import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Feedback } from 'src/model/feedback.model';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';

@Injectable({
  providedIn: 'root',
})
export class GroupChatDataService {
  private baseUrl = 'https://localhost:7191/api/';

  feedbackAdded = new BehaviorSubject<Feedback | null>(null);

  constructor(private http: HttpClient) { }
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

  sendMessageToGroupChat(token: string, personId: number, groupId: number, message: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChatMessage>(this.baseUrl + 'groupchat/message/add', {
      PersonId: personId,
      GroupChatId: groupId,
      Message: message
    }, { headers: headers });
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
