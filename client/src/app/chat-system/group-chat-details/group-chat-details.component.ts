import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { GroupChat } from 'src/model/groupchat.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-group-chat-details',
  templateUrl: './group-chat-details.component.html',
  styleUrls: ['./group-chat-details.component.css']
})
export class GroupChatDetailsComponent implements OnInit, OnDestroy{
  groupChatSubscription!:Subscription;
  getCurrentPersonSubscription!: Subscription;
  loggedPerson: Person = new Person();
  groupChat:GroupChat = new GroupChat();
  panelOpenState = false;
  descriptionOfGroup='lorem';

  private token: string = '';

  constructor(private utils:UtilsService, private dataStorage:DataStorageService){

  }

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
          this.groupChatSubscription = this.utils.groupChatPersonChat.subscribe((res)=>{
            if(res)
              this.groupChat = res;
              console.log(res);
          });
        });
      }
  }
  ngOnDestroy(): void {
    if(this.getCurrentPersonSubscription!=null) this.getCurrentPersonSubscription.unsubscribe();
    if(this.groupChatSubscription!=null) this.groupChatSubscription.unsubscribe();
    this.utils.selectChat.next(-1);
  }

}
