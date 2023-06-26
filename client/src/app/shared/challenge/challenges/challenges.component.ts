import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Challenge } from 'src/model/challenge.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditChallengeComponent } from '../add-edit-challenge/add-edit-challenge.component';
import { ToastrService } from 'ngx-toastr';
import { SeeSolutionsComponent } from '../see-solutions/see-solutions.component';
import { AddEditSolutionComponent } from '../add-edit-solution/add-edit-solution.component';

@Component({
  selector: 'app-challenges',
  templateUrl: './challenges.component.html',
  styleUrls: ['./challenges.component.css']
})
export class ChallengesComponent implements OnInit, OnDestroy {
  getChallengesSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  deleteChallengesSubscription!: Subscription;
  challengesSubscription!: Subscription;
  getSolutionSubecription!: Subscription;
  getRankingPositionSubscription!: Subscription;
  challenges: Challenge[] = [];
  canAddEdit: boolean = false;
  isOnChallanges:boolean=false;
  isIntern:boolean=false;
  text:string='';
  private token: string = '';

  constructor(
    private utils: UtilsService,
    private toastr: ToastrService,
    private dataStorage: DataStorageService,
    private dialog: MatDialog) {
  }


  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username, person.token).subscribe((res) => {
        this.canAddEdit = res.status == 'Trainer';
        this.isIntern = res.status =="Intern";
        this.getChallengesSubscription = this.dataStorage.getAllChallengesForPeople(this.token, res.status).subscribe((data) => {
          this.challenges = data;
          this.challenges.forEach(element => {
            element.canDelete = new Date(element.deadline) > new Date;
            this.getSolutionSubecription = this.dataStorage.getSolutionsForSpecificPersonForChallenge(this.token,res.personId, element.challangeId).subscribe((r) => {
              element.canAddSolution = r.filter(s=>s.approved==true).length>0? false:true;
            });
          });
          this.challengesSubscription = this.utils.isOnChallanges.subscribe((result)=>{
            this.isOnChallanges=result;
          });
          this.getRankingPositionSubscription = this.dataStorage.rankings(this.token).subscribe((rank)=>{
            let personRank = rank.find(p=>p.personId == res.personId);
            let posTest = personRank?.position ==1 ? "1st": personRank?.position == 2? "2nd":
            personRank?.position == 3? "3rd" : personRank?.position.toString()+"th";
            this.text = "You have "+ personRank?.points +" point and you are "+ posTest+" in the rankings!"
          })
        });
      });
    }
  }

  ngOnDestroy(): void {
    if (this.getChallengesSubscription != null) this.getChallengesSubscription.unsubscribe();
    if (this.deleteChallengesSubscription != null) this.deleteChallengesSubscription.unsubscribe();
    if (this.challengesSubscription != null) this.challengesSubscription.unsubscribe();
    if (this.getSolutionSubecription != null) this.getSolutionSubecription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.getRankingPositionSubscription != null) this.getRankingPositionSubscription.unsubscribe();
  }

  openDialog(op: number) {
    const dialogRef = op == 1 ? 
    this.dialog.open(AddEditChallengeComponent) 
    : op==2? this.dialog.open(SeeSolutionsComponent)
    : this.dialog.open(AddEditSolutionComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.challengeToEdit.next(null);
    this.openDialog(1);
  }

  onEdit(challenge: Challenge) {
    this.utils.challengeToEdit.next(challenge);
    this.openDialog(1);
  }

  onDelete(challenge: Challenge) {
    this.deleteChallengesSubscription = this.dataStorage.deleteChallenge(this.token, challenge.challangeId).subscribe(() => {
      this.toastr.success("Note was deleted succesfully!");
      let index = this.challenges.findIndex(c => c.challangeId == challenge.challangeId);
      if (index != -1)
        this.challenges.splice(index, 1);
    }, (error) => {
      this.toastr.error(error.error);
    })
  }

  onSeeAllSolutions(challenge: Challenge) {
    this.utils.challengeIdForSolutionsToEdit.next(challenge.challangeId);
    this.openDialog(2);
  }

  onPostSolution(challenge: Challenge){
    this.utils.challengeIdForSolutionsToEdit.next(challenge.challangeId);
    this.openDialog(3);
  }

  onRanking(){
    this.isOnChallanges=false;
  }

}
