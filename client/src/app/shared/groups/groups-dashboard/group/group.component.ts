import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css'],
})
export class GroupComponent implements OnInit, OnDestroy {
  groupSubscription!: Subscription;
  group: Group = new Group();
  person:Person=new Person();
  description:string='Lorem ipsum dolor sit amet, ';
  private token: string = '';

  constructor(
    private dataService: DataStorageService,
    private route: ActivatedRoute,
    private utils: UtilsService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.route.params.subscribe((params: Params) => {
        let id = +params['id'];
        this.groupSubscription = this.dataService
        .getGroupById(this.token, id)
        .subscribe((res) => {
          if (res != null){
            this.group = res;
            this.person=res.trainer;
          }
        });
      });
    }
  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  edit(){
    console.log("Click");
  }
}
