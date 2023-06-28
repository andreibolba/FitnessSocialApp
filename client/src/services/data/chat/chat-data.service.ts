import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { Message } from 'src/model/message.model';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root',
})
export class ChatDataService {
  private baseUrl = 'https://localhost:7191/api/';
  private hubUrl = 'https://localhost:7191/hubs/';
  private hubConnection?: HubConnection;
  newChat = new BehaviorSubject<Message | null>(null);
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient) { }

  getLastMessagesFormChatForCurrentPerson(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Message[]>(this.baseUrl + 'chat/' + personId, { headers: headers });
  }

  getAllMessagesFromAChat(token: string, currentPersonId: number, chatPersonId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Message[]>(this.baseUrl + 'chat/messages/' + currentPersonId + '/' + chatPersonId, { headers: headers });
  }

  async addMessage(token: string, message: Message) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.hubConnection?.invoke('AddMessage', {
      PersonSenderId: message.personSenderId,
      PersonReceiverId: message.personReceiverId,
      Message: message.message
    }).catch(error=>console.log(error));
  }

  deleteChat(token: string, personId: number, chatPersonId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'chat/deletechat/' + personId + '/' + chatPersonId, {}, { headers: headers });
  }

  createHubConnection(user: Person, otherUsername: string, token: string) {
    console.log(this.hubUrl);
    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl + 'message?user=' + otherUsername, {
      accessTokenFactory: () => token
    })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          this.messageThreadSource.next([...messages, message]);
          this.newChat.next(message);
        }
      });
    })

  }

  stopHubConnetion() {
    if (this.hubConnection)
      this.hubConnection.stop();
  }
  
}
