import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Note } from 'src/model/note.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditNoteComponent } from '../../notes/add-edit-note/add-edit-note.component';
import { AddEditFeedbackComponent } from '../add-edit-feedback/add-edit-feedback.component';
import { Feedback } from 'src/model/feedback.model';
import { SeeFeedbackComponent } from '../see-feedback/see-feedback.component';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.css']
})
export class FeedbackComponent implements OnInit, OnDestroy {
  myFeedbackSubscription!: Subscription;
  feedbacksForMeSubscription!: Subscription;
  personSubscription!: Subscription;
  deletefeedbackSubscription!: Subscription;
  myFeedbacks:Feedback[]=[];
  feedbacksForMe:Feedback[]=[];
  private token: string = '';
  username: string='';

  constructor(
    private utils: UtilsService, 
    private dataStorage: DataStorageService, 
    private dialog: MatDialog,
    private toastr: ToastrService
    ) {
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.username = person.username;
      this.personSubscription = this.dataStorage.personData.getPerson(this.username, this.token).subscribe((data)=>{
        this.myFeedbackSubscription = this.dataStorage.getAllFeedbacksForSpecificForPerson(this.token,data.personId,"sender").subscribe((res)=>{
          this.myFeedbacks=res;
        });
        this.feedbacksForMeSubscription = this.dataStorage.getAllFeedbacksForSpecificForPerson(this.token,data.personId,"receiver").subscribe((res)=>{
          this.feedbacksForMe=res;
        });
      });
    }
  }

  ngOnDestroy(): void {
    if (this.deletefeedbackSubscription != null) this.deletefeedbackSubscription.unsubscribe();
    if (this.myFeedbackSubscription != null) this.myFeedbackSubscription.unsubscribe();
    if (this.feedbacksForMeSubscription != null) this.feedbacksForMeSubscription.unsubscribe();
    if (this.personSubscription != null) this.personSubscription.unsubscribe();
  }

  openDialog(op:number) {
    const dialogRef = op==1? this.dialog.open(AddEditFeedbackComponent): this.dialog.open(SeeFeedbackComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.feedbackToEdit.next(null);
    this.openDialog(1);
  }

  onEdit(feedback: Feedback) {
    this.utils.feedbackToEdit.next(feedback);
    this.openDialog(1);
  }

  onDelete(feedback: Feedback) {
    this.deletefeedbackSubscription = this.dataStorage.deleteFeedback(this.token, feedback.feedbackId).subscribe(() => {
      this.toastr.success("Feedback was deleted succesfully!");
      let index = this.myFeedbacks.findIndex(c=>c.feedbackId == feedback.feedbackId);
      if(index!=-1)
        this.myFeedbacks.splice(index,1);
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  onSee(feedback:Feedback){
    this.utils.feedbackToEdit.next(feedback);
    this.openDialog(2);
  }
}
