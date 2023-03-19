import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-start-test',
  templateUrl: './start-test.component.html',
  styleUrls: ['./start-test.component.css'],
})
export class StartTestComponent implements OnInit, OnDestroy {
  testToStartSub!: Subscription;
  peopleSub!: Subscription;
  private token: string = '';
  private internId: number = -1;

  test: Test = new Test();

  constructor(private utils: UtilsService, private data: DataStorageService) {}

  ngOnDestroy(): void {
    if (this.testToStartSub != null) this.testToStartSub.unsubscribe();
    if (this.peopleSub != null) this.peopleSub.unsubscribe();
  }
  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.peopleSub = this.data
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          if (data) this.internId = data.personId;
        });

      this.testToStartSub = this.utils.testToStart.subscribe((res) => {
        if (res) this.test = res;
        console.log(this.test);
      });
    }
  }
}
