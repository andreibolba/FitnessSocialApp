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
  challenges: Challenge[] = [];
  canAddEdit: boolean = false;
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
      this.getPersonSubscription = this.dataStorage.getPerson(person.username, person.token).subscribe((res) => {
        this.canAddEdit = res.status == 'Trainer';
        this.getChallengesSubscription = this.dataStorage.getAllChallenges(this.token).subscribe((data) => {
          this.challenges = data;
          this.challenges.forEach(element => {
            element.canDelete = new Date(element.deadline) > new Date;
            console.log(element.canDelete);
          });
        });
      });
    }
  }

  ngOnDestroy(): void {
    if (this.getChallengesSubscription != null) this.getChallengesSubscription.unsubscribe();
    if (this.deleteChallengesSubscription != null) this.deleteChallengesSubscription.unsubscribe();
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

}
