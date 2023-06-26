import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Note } from 'src/model/note.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-add-edit-note',
  templateUrl: './add-edit-note.component.html',
  styleUrls: ['./add-edit-note.component.css']
})
export class AddEditNoteComponent implements OnInit, OnDestroy {
  getNoteSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  addEditSubscription!: Subscription;
  title: string = '';
  content: string = '';
  operation: string = '';
  person: Person = new Person();
  private token: string = '';
  private noteId: number = -1;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialogRef<AddEditNoteComponent>,
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
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username, this.token).subscribe((data) => {
        if (data) {
          this.person = data;
        }
        this.getNoteSubscription = this.utils.noteToEdit.subscribe((res) => {
          if (res) {
            this.operation = "Edit";
            this.title = res.noteTitle;
            this.content = res.noteBody;
            this.noteId = res.noteId;
          } else {
            this.operation = "Add";
          }
        });
      });

    }
  }

  ngOnDestroy(): void {
    if (this.getNoteSubscription != null) this.getNoteSubscription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.addEditSubscription != null) this.addEditSubscription.unsubscribe();
  }

  onSignUpSubmit() {
    let note = new Note();
    note.noteTitle = this.title;
    note.noteBody = this.content;
    note.personId = this.person.personId;
    if (this.operation == "Add") {
      this.addEditSubscription = this.dataStorage.createNote(this.token,note).subscribe(()=>{
        this.toastr.success("Note was added succesfully!");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }else{
      note.noteId = this.noteId;
      this.addEditSubscription = this.dataStorage.updateNote(this.token,note).subscribe(()=>{
        this.toastr.success("Note was updated succesfully!");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }
  }

}
