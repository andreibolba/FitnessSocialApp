import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ChallengeSolution } from 'src/model/challengesolution.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { SolutionPointsComponent } from '../solution-points/solution-points.component';

@Component({
  selector: 'app-see-solutions',
  templateUrl: './see-solutions.component.html',
  styleUrls: ['./see-solutions.component.css']
})
export class SeeSolutionsComponent implements OnInit, OnDestroy {
  getSolutionsSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  deleteSubscription!:Subscription;
  getIdSubscription!: Subscription;
  approveSubscription!: Subscription;
  declineSubscription!: Subscription;
  pointsSubscription!: Subscription;
  challengeSubscription!: Subscription;
  solutions: ChallengeSolution[] = [];
  isReadOnly:boolean=true;
  point:number=0;
  private personId:number=-1;
  private maxPoints:number=-1;
  private token: string = '';

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dial: MatDialogRef<SeeSolutionsComponent>,
    private toastr: ToastrService,
    private dialog: MatDialog
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
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username,person.token).subscribe((data)=>{
        this.personId = data.personId;
        this.isReadOnly =  data.status == "Intern";
        if(this.isReadOnly){
          this.getIdSubscription = this.utils.challengeIdForSolutionsToEdit.subscribe((res) => {
            this.getSolutionsSubscription = this.dataStorage.getSolutionsForSpecificPersonForChallenge(this.token,data.personId, res).subscribe((data) => {
              this.solutions = data;
            });
          });
        }else{
          this.getIdSubscription = this.utils.challengeIdForSolutionsToEdit.subscribe((res) => {
            this.challengeSubscription = this.dataStorage.getAllChallengesForPeople(this.token, data.status).subscribe((ch)=>{
              let challenge = ch.find(f=>f.challangeId == res);
              this.maxPoints = challenge?.points!;
            });
            this.getSolutionsSubscription = this.dataStorage.getSolutionsForSpecificChallenge(this.token, res).subscribe((data) => {
              this.solutions = data;
            });
          });
        }
      });
    }
  }
  ngOnDestroy(): void {
    if (this.getIdSubscription != null) this.getIdSubscription.unsubscribe();
    if (this.getSolutionsSubscription != null) this.getSolutionsSubscription.unsubscribe();
    if (this.approveSubscription != null) this.approveSubscription.unsubscribe();
    if (this.declineSubscription != null) this.declineSubscription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.deleteSubscription != null) this.getPersonSubscription.unsubscribe();
  }

  approve(solution: ChallengeSolution) {
    this.utils.pointToSolutionApprove.next(this.point);
    this.utils.maxpointToSolutionApprove.next(this.maxPoints);
    this.openDialog(solution);
  }

  decline(solution: ChallengeSolution) {
    this.declineSubscription = this.dataStorage.declineSolution(this.token, solution.challangeSolutionId).subscribe(() => {
      this.toastr.success("Solution was declined!");
      solution.approved = false;
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  delete(solution: ChallengeSolution) {
    this.deleteSubscription = this.dataStorage.deleteSolution(this.token, solution.challangeSolutionId).subscribe(() => {
      this.toastr.success("Solution was declined!");
      solution.approved = false;
      this.dial.close();
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  downloadFile(solid: number) {
    this.dataStorage.downloadFile(this.token, solid);
  }

  openDialog(solution: ChallengeSolution) {
    const dialogRef = this.dialog.open(SolutionPointsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
      this.pointsSubscription = this.utils.pointToSolutionApprove.subscribe((data) => {
        if (data) {
          this.approveSubscription = this.dataStorage.approveSolution(this.token, solution.challangeSolutionId, data).subscribe(() => {
            this.toastr.success("Solution was accepted!");
            solution.approved = true;
            solution.points = data;
          }, (error) => {
            this.toastr.error(error);
            console.log(error);
          });
        }
      });
    });
  }

}
