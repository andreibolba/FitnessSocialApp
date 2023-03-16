import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditMeetingDialogComponent } from '../edit-meeting-dialog/edit-meeting-dialog.component';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css'],
})
export class MeetingComponent implements OnInit, OnDestroy {
  isTrainer: boolean = true;
  private token: string = '';

  personSub!: Subscription;
  meetingSub!: Subscription;

  constructor(
    private utils: UtilsService,
    private dialog: MatDialog,
    private dataService: DataStorageService
  ) {}
  ngOnDestroy(): void {
    if (this.personSub != null) this.personSub.unsubscribe();
    if (this.meetingSub != null) this.meetingSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      let id = -1;
      this.personSub = this.dataService
        .getPerson(person.username, this.token)
        .subscribe(
          (data) => {
            id = data.personId;
            this.isTrainer = data.status == 'Trainer';
          },
          () => {},
          () => {}
        );
    }
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditMeetingDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.meetingToEdit.next(null);
    this.openDialog();
  }
}
