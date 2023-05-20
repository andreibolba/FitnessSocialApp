import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Message } from 'src/model/message.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-chat-messages',
  templateUrl: './chat-messages.component.html',
  styleUrls: ['./chat-messages.component.css'],
})
export class ChatMessagesComponent implements OnInit, OnDestroy {
  date = new Date();
  getCurrentPersonSubscription!:Subscription;
  getMessagesFromChatSubscription!:Subscription;
  getChatPersonIdSubcription!:Subscription;
  allChats:Message[]=[];
  chatPerson:Person=new Person();

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.getCurrentPersonSubscription = this.dataStorage.getPerson(person.username,person.token).subscribe((data)=>{
        this.getChatPersonIdSubcription = this.utils.chatPersonChat.subscribe((personId)=>{
          this.getMessagesFromChatSubscription = this.dataStorage
            .getAllMessagesFromAChat(person.token,data.personId,personId)
            .subscribe((res) => {
              this.allChats = res;
              this.chatPerson = this.allChats[0].personReceiverId == data.personId? this.allChats[0].personSender : this.allChats[0].personReceiver;
            },()=>{},()=>{
              setTimeout(() => {
              var objDiv = document.getElementById("chat_content");
              if(objDiv)
                objDiv.scrollTop = objDiv.scrollHeight - objDiv.clientHeight;
              },0);
            });
        });
      });
    }
  }
  
  ngOnDestroy(): void {
    if(this.getCurrentPersonSubscription!=null)this.getCurrentPersonSubscription.unsubscribe();
    if(this.getChatPersonIdSubcription!=null)this.getChatPersonIdSubcription.unsubscribe();
    if(this.getMessagesFromChatSubscription!=null)this.getMessagesFromChatSubscription.unsubscribe();
  }
}
