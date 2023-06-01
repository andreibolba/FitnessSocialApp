import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditNoteComponent } from '../add-edit-note/add-edit-note.component';
import { Note } from 'src/model/note.model';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent implements OnInit, OnDestroy {
  notesSubscription!: Subscription;
  deleteNoteSubscription!: Subscription;
  notes:Note[]=[];
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
      this.notesSubscription = this.dataStorage.getAllNotes(this.token).subscribe((res)=>{
        this.notes=res;
        console.log(res);
      });
    }
  }

  ngOnDestroy(): void {
    if (this.deleteNoteSubscription != null) this.deleteNoteSubscription.unsubscribe();
    if (this.notesSubscription != null) this.notesSubscription.unsubscribe();
  }

  openDialog() {
    const dialogRef = this.dialog.open(AddEditNoteComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.noteToEdit.next(null);
    this.openDialog();
  }

  onEdit(note: Note) {
    this.utils.noteToEdit.next(note);
    this.openDialog();
  }

  onDelete(note: Note) {
    this.deleteNoteSubscription = this.dataStorage.deleteNote(this.token, note.noteId).subscribe(() => {
      this.toastr.success("Note was deleted succesfully!");
      let index = this.notes.findIndex(c=>c.noteId == note.noteId);
      if(index!=-1)
        this.notes.splice(index,1);
    }, (error) => {
      this.toastr.error(error.error);
    })
  }

}
