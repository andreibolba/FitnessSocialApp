import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Question } from 'src/model/question.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit, OnDestroy{
  panelOpenState = false;
  questions:Question[] | null=null;
  dataGroupSub!: Subscription;
  token:string='';

  constructor(
    private dataService: DataStorageService,
    private dialog: MatDialog,
    private utils: UtilsService){
  }

  onAdd(){
    this.utils.groupToEdit.next(null);
    this.openDialog();
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
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
      let id=-1;
      this.dataService.getPerson(person.username,this.token).subscribe((data)=>{
        id=data.personId;
      },()=>{},()=>{
        this.dataGroupSub = this.dataService
          .getAllQuestionsByTrainer(person.token,id)
          .subscribe((data) => {
            this.questions=data;
          });
      });
    }
  }
}
