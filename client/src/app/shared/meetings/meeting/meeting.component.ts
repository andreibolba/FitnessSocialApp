import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Meeting } from 'src/model/meeting.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditMeetingDialogComponent } from '../edit-meeting-dialog/edit-meeting-dialog.component';
import { SeeMeetingParticipantsComponent } from '../see-meeting-participants/see-meeting-participants.component';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css'],
})
export class MeetingComponent implements OnInit, OnDestroy {
  isTrainer: boolean = true;
  isFromGroup = true;
  allMeetings: Meeting[] = [];
  participants: string = ' and other ';
  private token: string = '';

  personSub!: Subscription;
  fromGroupSub!: Subscription;
  meetingSub!: Subscription;
  meetingsSub!: Subscription;
  deleteSub!: Subscription;
  mainId: string = '';
  buttonsClass: string = '';
  isInGroup: boolean = false;

  constructor(
    private utils: UtilsService,
    private dialog: MatDialog,
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router:Router
  ) {}
  ngOnDestroy(): void {
    if (this.personSub != null) this.personSub.unsubscribe();
    if (this.meetingSub != null) this.meetingSub.unsubscribe();
    if (this.deleteSub != null) this.deleteSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.fromGroupSub = this.utils.isFromGroupDashboard.subscribe(
        (res) => {
          this.isFromGroup = res || !this.router.url.endsWith('meetings');
          if (this.isFromGroup) {
            this.mainId = 'maingroup';
            this.buttonsClass = 'buttonsgroup';
            this.isFromGroup = true;
            this.route.params.subscribe((params: Params) => {
              let groupId = +params['id'];
              this.personSub = this.dataService.personData
              .getPerson(person.username, this.token)
              .subscribe((data) => {
                this.isTrainer = data.status == 'Trainer';
                this.meetingsSub = this.dataService.meetingData.getAllMeetingsForPerson(person.token,groupId,"group")
                  .subscribe((res) => {
                    this.allMeetings = this.getAllMeetings(res);
                  });
              });
            });

          } else {
            this.mainId = 'main';
            this.buttonsClass = 'buttons';
            this.isFromGroup = false;
            this.personSub = this.dataService.personData
            .getPerson(person.username, this.token)
            .subscribe((data) => {
              this.isTrainer = data.status == 'Trainer';
              this.meetingsSub = this.dataService.meetingData
                .getAllMeetingsForPerson(
                  this.token,
                  data.personId,
                  data.status.toLocaleLowerCase()
                )
                .subscribe((res) => {
                  this.allMeetings = this.getAllMeetings(res);
                });
            });

          }
        }
      );
      this.meetingSub = this.dataService.meetingData.meetignAdded.subscribe((res)=>{
        if(res){
          let index=this.allMeetings.findIndex(m=>m.meetingId==res.meetingId);
          if(index==-1){
            this.allMeetings.unshift(res);
          }else{
            this.allMeetings[index]=res;
          }
        }
      });
    }
  }

  openDialog(op: number) {
    const dialogRef =
      op == 1
        ? this.dialog.open(EditMeetingDialogComponent)
        : this.dialog.open(SeeMeetingParticipantsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.meetingToEdit.next(null);
    this.openDialog(1);
  }

  onEdit(meeting: Meeting) {
    this.utils.meetingToEdit.next(meeting);
    this.openDialog(1);
  }

  seeMeeting(people: Person[]) {
    this.utils.meetingParticipants.next(people);
    this.openDialog(2);
  }

  onDelete(meetingId: number) {
    this.deleteSub = this.dataService.meetingData
      .deleteMeeting(this.token, meetingId)
      .subscribe(
        () => {
          let index=this.allMeetings.findIndex(m=>m.meetingId==meetingId);
          this.allMeetings.splice(index,1);
          this.toastr.success('Deleted was made succesfully!');
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
  }

  private getAllMeetings (allMeetings:Meeting[]){
    allMeetings.forEach((element) => {
      element.participants = '';
      if (element.allPeopleInMeeting.length == 0)
        element.participants = 'No participants';
      else if (element.allPeopleInMeeting.length >= 3) {
        element.participants +=
          element.allPeopleInMeeting[0].firstName +
          ' ' +
          element.allPeopleInMeeting[0].lastName +
          ', ';
        element.participants +=
          element.allPeopleInMeeting[1].firstName +
          ' ' +
          element.allPeopleInMeeting[1].lastName;
      } else {
        element.allPeopleInMeeting.forEach((person) => {
          element.participants +=
            person.firstName + ' ' + person.lastName + ', ';
        });
      }
    });
    return allMeetings;
  }
}
