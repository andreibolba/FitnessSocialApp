import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { EditDialogComponent } from 'src/app/shared/questions/edit-dialog/edit-dialog.component';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Question } from 'src/model/question.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-participants',
  templateUrl: './participants.component.html',
  styleUrls: ['./participants.component.css']
})
export class ParticipantsComponent implements OnInit, OnDestroy{
  panelOpenState = false;
  questions:Question[] | null | undefined =null;
  people:Person[] | null | undefined =null;
  dataGroupSub!: Subscription;
  dataPeopleSub!: Subscription;
  token:string='';

  constructor(
    private dataService: DataStorageService,
    private route: ActivatedRoute,
    private utils: UtilsService){
  }

  ngOnDestroy(): void {
    if(this.dataGroupSub!=null) this.dataGroupSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      let groupId=-1;
      this.route.params.subscribe((params: Params) => {
        groupId = +params['id'];
        console.log(groupId);
        this.dataPeopleSub = this.dataService.getAllPeopleInGroup(this.token,groupId).subscribe((data)=>{
          this.people=data;
          console.log(data);
        });
      });

    }
  }
}
