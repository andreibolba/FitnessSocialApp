import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

export class SearchPeople {
  public id: number;
  public username: string;
  public firstName: string;
  public lastName: string;

  constructor() {
    this.username = '';
    this.firstName = '';
    this.lastName = '';
    this.id = -1;
  }
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  haveChat = -1;
  haveChatSubscription!: Subscription;
  getPeopleSubscription!: Subscription;
  getCurrentPersonSubscription!: Subscription;
  searchPeople: SearchPeople[] = [];
  searchPersonSelect: SearchPeople[] = [];
  searchText: string = '';
  loggedPerson: Person = new Person();

  constructor(private utils: UtilsService, private dataStorage: DataStorageService) {

  }

  ngOnInit(): void {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.getCurrentPersonSubscription = this.dataStorage.personData
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.loggedPerson=data;
          this.haveChatSubscription = this.utils.selectChat.subscribe((data) => {
            this.haveChat = data;
            this.getPeopleSubscription = this.dataStorage.personData.getPeople(person.token).subscribe((res) => {
              res.forEach(element => {
                let searchPerson = new SearchPeople();
                searchPerson.id = element.personId;
                searchPerson.username = element.username;
                searchPerson.firstName = element.firstName;
                searchPerson.lastName = element.lastName;

                this.searchPeople.push(searchPerson);
              })
            });
          });
        });
    }
  }

  ngOnDestroy(): void {
    if (this.haveChatSubscription != null) this.haveChatSubscription.unsubscribe();
  }

  onSend() {
    let value = +(<HTMLSelectElement>document.getElementById('people')).value;
    this.utils.chatPersonChat.next(value);
    this.utils.selectChat.next(1);
  }
}
