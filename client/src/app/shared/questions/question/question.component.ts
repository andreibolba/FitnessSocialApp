import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Question } from 'src/model/question.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';

@Component({
  selector: 'app-questions',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionsComponent implements OnInit, OnDestroy{
  panelOpenState = false;
  deleteQuestion!:Subscription;
  questions:Question[]=[];
  dataGroupSub!: Subscription;
  questionSub!: Subscription;
  token:string='';

  constructor(
    private dataService: DataStorageService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private utils: UtilsService){
  }

  onAdd(){
    this.utils.questionToEdit.next(null);
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

  deletequestion(questionId:number){
      this.deleteQuestion = this.dataService.quizData.questionData.deleteQuestion(this.token,questionId).subscribe(()=>{
        this.toastr.success("A question was deleted succesfully!");
        let index=this.questions.findIndex(q=>q.questionId==questionId);
        this.questions.splice(index,1);
      },(error)=>{
        this.toastr.error(error.error);
      });
  }

  edit(question:Question){
    this.utils.questionToEdit.next(question);
    this.openDialog();
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
      this.dataService.personData.getPerson(person.username,this.token).subscribe((data)=>{
        id=data.personId;
      },()=>{},()=>{
        this.dataGroupSub = this.dataService.quizData.questionData
          .getAllQuestionsByTrainer(person.token,id)
          .subscribe((data) => {
            this.questions=data;
          });
      });
      this.questionSub = this.dataService.quizData.questionData.questionAdded.subscribe((res)=>{
        if(res){
          let index = this.questions.findIndex(t=>t.questionId == res.questionId);
          if(index==-1){
            this.questions.unshift(res);
          }else{
            this.questions[index]=res;
          }
        }
      });
    }
  }
}
