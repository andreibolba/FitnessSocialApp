import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, elementAt } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Message } from 'src/model/message.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { MatDialog } from '@angular/material/dialog';
import { AddEditGroupChatComponent } from '../add-edit-group-chat/add-edit-group-chat.component';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { GroupChat } from 'src/model/groupchat.model';
import { PresenceService } from 'src/services/presence.service';

@Component({
  selector: 'app-all-chats',
  templateUrl: './all-chats.component.html',
  styleUrls: ['./all-chats.component.css'],
})
export class AllChatsComponent implements OnInit, OnDestroy {
  selectChatSubscription!: Subscription;
  getCurrentPersonSubscription!: Subscription;
  getAllChatsSubscription!: Subscription;
  getAllGroupChatsSubscription!: Subscription;
  allChats: Message[] = [];
  allGroupChats: GroupChatMessage[] = [];
  currentPerson: Person = new Person();
  newChatSubscription!: Subscription;
  newGroupChatSubscription!: Subscription;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialog,
    private presenceService:PresenceService
  ) { }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.getCurrentPersonSubscription = this.dataStorage
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.presenceService.createHubConnection(data,person.token);
          this.currentPerson = data;
          this.getAllChatsSubscription = this.dataStorage
            .getLastMessagesFormChatForCurrentPerson(
              person.token,
              data.personId
            )
            .subscribe((res) => {
              this.allChats = res;
              this.allChats.forEach((element) => {
                element.chatPerson =
                  element.personReceiverId == data.personId
                    ? element.personSender
                    : element.personReceiver;
                element.youOrThem =
                  element.personReceiverId == data.personId ? 'He: ' : 'You: ';
              });
            });
          this.getAllGroupChatsSubscription = this.dataStorage
            .getAllGroupChatsLastMessages(person.token, data.personId)
            .subscribe((chats) => {
              this.allGroupChats = chats;
            });
        });
      this.newChatSubscription = this.dataStorage.newChat.subscribe(
        (res) => {
          if (res != null) {
            res.chatPerson =
              res.personReceiverId == this.currentPerson.personId
                ? res.personSender
                : res.personReceiver;
            res.youOrThem =
              res.personReceiverId == this.currentPerson.personId
                ? 'He: '
                : 'You: ';
            let index = this.allChats.findIndex(
              (c) =>
                ((c.chatId == res.chatId)||(c.chatPerson.personId == res.chatPerson.personId))
            );
            if (index != -1)
              this.allChats.splice(index, 1);
            if (res.message != '')
              this.allChats.unshift(res);
          }
        },
        () => { },
        () => {
          this.dataStorage.newChat.next(null);
        }
      );
      this.newGroupChatSubscription = this.utils.newGroupChatMessage.subscribe(
        (res) => {
          if (res != null) {
            let index = this.allGroupChats.findIndex(
              (c) =>
                c.groupChatId == res.groupChatId
            );
            if (index != -1)
              this.allGroupChats.splice(index, 1);
            if (res.message != '')
              this.allGroupChats.unshift(res);
          }
        },
        () => { },
        () => {
          this.utils.newGroupChatMessage.next(null);
        }
      );
    }
  }

  ngOnDestroy(): void {
    if (this.selectChatSubscription != null)
      this.selectChatSubscription.unsubscribe();
    if (this.getAllChatsSubscription != null)
      this.getAllChatsSubscription.unsubscribe();
    if (this.getAllGroupChatsSubscription != null)
      this.getAllGroupChatsSubscription.unsubscribe();
  }

  onChatClick(chatPersonId: number) {
    this.utils.chatPersonChat.next(chatPersonId);
    this.utils.selectChat.next(1);
  }

  onGroupChatClick(groupchat: GroupChat) {
    this.utils.selectChat.next(2);
    this.utils.groupChatPersonChat.next(groupchat);
  }

  openDialog() {
    const dialogRef = this.dialog.open(AddEditGroupChatComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  addGroup() {
    this.utils.editGroupChatOption.next(1);
    this.openDialog();
  }

  isOnline(usename:string):boolean{
    let online=false;
    this.presenceService.onlineUsers$.pipe()
    this.presenceService.onlineUsers$.forEach(element=>{
      if(element.includes(usename))
        online=true;
    });
    return online;
  }
}
