import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Message } from 'src/model/message.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-all-chats',
  templateUrl: './all-chats.component.html',
  styleUrls: ['./all-chats.component.css']
})
export class AllChatsComponent implements OnInit,OnDestroy{
  selectChatSubscription!:Subscription;
  getCurrentPersonSubscription!:Subscription;
  getAllChatsSubscription!:Subscription;
  allChats:Message[]=[];
  currentPerson:Person=new Person();
  newChatSubscription!:Subscription;

  constructor(private utils:UtilsService,private dataStorage:DataStorageService){
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.getCurrentPersonSubscription = this.dataStorage.getPerson(person.username,person.token).subscribe((data)=>{
        this.currentPerson=data;
        this.getAllChatsSubscription = this.dataStorage
          .getLastMessagesFormChatForCurrentPerson(person.token,data.personId)
          .subscribe((res) => {
            this.allChats = res;
            this.allChats.forEach(element => {
              element.chatPerson = element.personReceiverId == data.personId? element.personSender : element.personReceiver;
              element.youOrThem = element.personReceiverId == data.personId? 'He: ' : 'You: ';
            });
          });
      });
      this.newChatSubscription = this.utils.newChat.subscribe((res)=>{
        if(res!=null){
          res.chatPerson = res.personReceiverId == this.currentPerson.personId? res.personSender : res.personReceiver;
          res.youOrThem = res.personReceiverId == this.currentPerson.personId? 'He: ' : 'You: ';
          let index = this.allChats.findIndex(c=>c.personReceiverId === res.personReceiverId && c.personSenderId === res.personSenderId);
          this.allChats.splice(index,1);
          this.allChats.unshift(res);
        }
      },()=>{},()=>{
        this.utils.newChat.next(null);
      });
    }
  }


  ngOnDestroy(): void {
    if(this.selectChatSubscription!=null) this.selectChatSubscription.unsubscribe();
    if(this.getAllChatsSubscription!=null) this.getAllChatsSubscription.unsubscribe();
  }

  onChatClick(chatPersonId:number){
    this.utils.chatPersonChat.next(chatPersonId);
    this.utils.selectChat.next(1);
  }

  onGroupChatClick(){
    this.utils.selectChat.next(2);
  }
}
