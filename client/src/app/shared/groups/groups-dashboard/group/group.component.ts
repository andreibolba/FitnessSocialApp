import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgModel } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { EditGroupDialogComponent } from 'src/app/admin/edit-group-dialog/edit-group-dialog.component';
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
  tests:number=-1;
  participants:number=-1;
  tasks:number=-1;
  challanges:number=-1;
  canEdit:boolean=false;
  private token: string = '';

  constructor(
    private dataService: DataStorageService,
    private route: ActivatedRoute,
    private utils: UtilsService,
    private dialog: MatDialog
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
        .subscribe((res:any) => {
          if (res != null){
            this.tests=res.tests;
            this.participants=res.participants;
            this.group=res.group;
            this.person = res.group.trainer;
            this.canEdit = person.username == res.group.trainer.username;
          }
        });
      });
    }
  }
  ngOnDestroy(): void {
    if(this.groupSubscription!=null) this.groupSubscription.unsubscribe();
  }

  edit(){
    console.log("Click");
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditGroupDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  editGroup(group:Group){
    this.utils.groupToEdit.next(group);
    this.openDialog();
  }
}
