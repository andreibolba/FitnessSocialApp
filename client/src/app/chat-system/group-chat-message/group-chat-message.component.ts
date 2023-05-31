import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription, ignoreElements } from 'rxjs';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-group-chat-message',
  templateUrl: './group-chat-message.component.html',
  styleUrls: ['./group-chat-message.component.css'],
})
export class GroupChatMessageComponent implements OnInit, OnDestroy {
  getGroupSubscription!: Subscription;
  getCurrentPersonSubscription!: Subscription;
  addMessageSubscription!: Subscription;
  getMessagesSubscription!: Subscription;
  sendMessagesSubscription!: Subscription;
  loggedPerson: Person = new Person();
  groupChat: GroupChat = new GroupChat();
  messages: GroupChatMessage[] = [];
  participants: string = '';
  message: string = '';

  private token: string = '';
  private personId: number = -1;
  private groupId: number = -1;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private taostr: ToastrService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getCurrentPersonSubscription = this.dataStorage
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.loggedPerson = data;
          this.personId = data.personId;
          this.getGroupSubscription = this.utils.groupChatPersonChat.subscribe(
            (res) => {
              if (res) {
                this.groupChat = res;
                this.groupId = res.groupChatId;
                this.participants = '';
                if (this.groupChat.participants.length >= 3) {
                  this.participants +=
                    this.groupChat.participants[0].firstName +
                    ' ' +
                    this.groupChat.participants[0].lastName +
                    ', ';
                  this.participants +=
                    this.groupChat.participants[1].firstName +
                    ' ' +
                    this.groupChat.participants[1].lastName;
                } else {
                  this.groupChat.participants.forEach((person) => {
                    this.participants +=
                      person.firstName + ' ' + person.lastName + ', ';
                  });
                }
                this.getMessagesSubscription = this.dataStorage
                  .getAllMessagesForGroup(this.token, res.groupChatId)
                  .subscribe((mess) => {
                    this.messages = mess;
                    setTimeout(() => {
                      var objDiv = document.getElementById('chat_content');
                      if (objDiv) {
                        objDiv.scrollTop = objDiv.scrollHeight - objDiv.clientHeight;
                      }
                    }, 0);
                  });                 
              }
            }
          );
        });
        
    }
  }

  ngOnDestroy(): void {
    if (this.getGroupSubscription != null)
      this.getGroupSubscription.unsubscribe();
    if (this.getCurrentPersonSubscription != null)
      this.getCurrentPersonSubscription.unsubscribe();
    if (this.addMessageSubscription != null)
      this.addMessageSubscription.unsubscribe();
    if (this.sendMessagesSubscription != null)
      this.sendMessagesSubscription.unsubscribe();
  }

  seeDetails(groupChat : GroupChat){
    this.utils.selectChat.next(3);
    this.utils.groupChatPersonChat.next(groupChat);
  }

  onSend() {
    this.sendMessagesSubscription = this.dataStorage
      .sendMessageToGroupChat(
        this.token,
        this.personId,
        this.groupId,
        this.message
      )
      .subscribe(
        (res) => {
          this.messages.push(res);
          this.utils.newGroupChatMessage.next(res);
          this.message = '';
          setTimeout(() => {
            var objDiv = document.getElementById('chat_content');
            if (objDiv)
              objDiv.scrollTop = objDiv.scrollHeight - objDiv.clientHeight;
          }, 0);
        },
        (error) => {
          this.taostr.error(error.error);
        }
      );
  }
}
