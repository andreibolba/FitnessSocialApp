import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Message } from 'src/model/message.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { PresenceService } from 'src/services/presence.service';
import { UtilsService } from 'src/services/utils.service';


@Component({
  selector: 'app-chat-messages',
  templateUrl: './chat-messages.component.html',
  styleUrls: ['./chat-messages.component.css'],
})
export class ChatMessagesComponent implements OnInit, OnDestroy {
  message: string = '';
  getCurrentPersonSubscription!: Subscription;
  getChatPersonSubscription!: Subscription;
  getMessagesFromChatSubscription!: Subscription;
  getChatPersonIdSubcription!: Subscription;
  addMessageSubscription!: Subscription;
  deleteChatSubscription!: Subscription;
  allChats: Message[] = [];
  chatPerson: Person = new Person();
  loggedPerson: Person = new Person();

  private token: string = '';

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private toastr: ToastrService,
    private router: Router,
    private presenceService: PresenceService
  ) {

  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getCurrentPersonSubscription = this.dataStorage.personData.getPerson(person.username, person.token).subscribe((data) => {
        this.loggedPerson = data;
        this.getChatPersonIdSubcription = this.utils.chatPersonChat.subscribe((personId) => {
          this.getChatPersonSubscription = this.dataStorage.personData.getPersonById(personId, this.token).subscribe((otherPerson) => {
            this.chatPerson = otherPerson;
            this.dataStorage.chatData.createHubConnection(data, otherPerson.username, person.token);
            this.dataStorage.chatData.messageThread$.forEach(element => {
              this.allChats = element;
            });
            setTimeout(() => {
              console.log("ok");
              var objDiv = document.getElementById("chat_content");
              if (objDiv)
                objDiv.scrollTop = objDiv.scrollHeight - objDiv.clientHeight;
            }, 540);
          })
        });
      });
    }
  }

  ngOnDestroy(): void {
    this.dataStorage.chatData.stopHubConnetion();
    if (this.getCurrentPersonSubscription != null) this.getCurrentPersonSubscription.unsubscribe();
    if (this.getChatPersonIdSubcription != null) this.getChatPersonIdSubcription.unsubscribe();
    if (this.getMessagesFromChatSubscription != null) this.getMessagesFromChatSubscription.unsubscribe();
  }

  onSend() {
    let messageToSend = new Message();
    messageToSend.personSenderId = this.loggedPerson.personId;
    messageToSend.personSender = this.loggedPerson;
    messageToSend.personReceiverId = this.chatPerson.personId;
    messageToSend.personReceiver = this.chatPerson;
    messageToSend.message = this.message;
    this.dataStorage.chatData.addMessage(this.token, messageToSend).then(() => {
      this.message = '';
      setTimeout(() => {
        var objDiv = document.getElementById("chat_content");
        if (objDiv)
          objDiv.scrollTop = objDiv.scrollHeight - objDiv.clientHeight;
      }, 540);
    });
  }

  delete() {
    this.utils.selectChat.next(-1);
    this.deleteChatSubscription = this.dataStorage.chatData.deleteChat(this.token, this.loggedPerson.personId, this.chatPerson.personId).subscribe(() => {
      this.toastr.success("Delete chat successfully!");
      this.utils.selectChat.next(-1);
      let lenght = this.allChats.length;
      let mess = this.allChats[lenght - 1];
      mess.message = '';
      this.dataStorage.chatData.newChat.next(mess);
    }, (error) => {
      this.toastr.success(error.error);
    });
  }

  onDetails() {
    this.router.navigate(["dashboard/profile/" + this.chatPerson.personId]);
  }

  isOnline(usename: string): boolean {
    let online = false;
    this.presenceService.onlineUsers$.pipe()
    this.presenceService.onlineUsers$.forEach(element => {
      if (element.includes(usename))
        online = true;
    });
    return online;
  }
}
