import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-edit-meeting-dialog',
  templateUrl: './edit-meeting-dialog.component.html',
  styleUrls: ['./edit-meeting-dialog.component.css']
})
export class EditMeetingDialogComponent {
  editMode=true;


  onSignUpSubmit(fomr:NgForm){

  }
}
