import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Challenge } from 'src/model/challenge.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-add-edit-challenge',
  templateUrl: './add-edit-challenge.component.html',
  styleUrls: ['./add-edit-challenge.component.css']
})
export class AddEditChallengeComponent implements OnInit, OnDestroy {
  operation: string = '';
  getPersonSubscription!: Subscription;
  getChallengeSubscription!: Subscription;
  addEditChallengeSubscription!: Subscription;
  challengeId:number=-1;
  challengeName:string='';
  challengeDescription:string='';
  challengeDeadline:Date=new Date();
  challengePoints:number=0;
  today:Date=new Date();
  private token: string = '';
  private person: Person = new Person();

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialogRef<AddEditChallengeComponent>,
    private toastr: ToastrService) {

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
        this.getChallengeSubscription = this.utils.challengeToEdit.subscribe((res)=>{
          if(res){
            this.challengeName = res.challangeName;
            this.challengeDescription = res.challangeDescription;
            this.challengeDeadline = res.deadline;
            this.challengeId = res.challangeId;
            this.challengePoints = res.points;
            this.operation = "Edit";
          }else{
            this.operation="Add";
          }
        })
      });
    }
  }

  ngOnDestroy(): void {
    if(this.getPersonSubscription!=null) this.getPersonSubscription.unsubscribe();
    if(this.getChallengeSubscription!=null) this.getChallengeSubscription.unsubscribe();
    if(this.addEditChallengeSubscription!=null) this.addEditChallengeSubscription.unsubscribe();
  }

  onSignUpSubmit(){
    let challenge = new Challenge();
    challenge.challangeName = this.challengeName;
    challenge.challangeDescription = this.challengeDescription;
    challenge.deadline = this.challengeDeadline;
    challenge.trainerId = this.person.personId;
    challenge.points = this.challengePoints;
    if(this.operation=="Add"){
      this.addEditChallengeSubscription =this.dataStorage.challengeData.addChallenge(this.token,challenge).subscribe((res)=>{
        res.canDelete = true;
        this.dataStorage.challengeData.challengeAdded.next(res);
        this.toastr.success("Challenge was added succesfully!");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }else{
      challenge.challangeId = this.challengeId;
      this.addEditChallengeSubscription =this.dataStorage.challengeData.editChallenge(this.token,challenge).subscribe((res)=>{
        res.canDelete = true;
        this.dataStorage.challengeData.challengeAdded.next(res);
        this.toastr.success("Challenge was updated succesfully!");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }
  }

}
