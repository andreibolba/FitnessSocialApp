import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ChallengeSolution } from 'src/model/challengesolution.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { TaskSolution } from 'src/model/tasksolution.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-see-all-task-solutions',
  templateUrl: './see-all-task-solutions.component.html',
  styleUrls: ['./see-all-task-solutions.component.css']
})
export class SeeAllTaskSolutionsComponent implements OnInit, OnDestroy {
  getSolutionsSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  getIdSubscription!: Subscription;
  pointsSubscription!: Subscription;
  challengeSubscription!: Subscription;
  solutions: TaskSolution[] = [];
  private token: string = '';

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService
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
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username, person.token).subscribe((data) => {
        this.getIdSubscription = this.utils.taskIdToUpload.subscribe((res) => {
          this.getSolutionsSubscription = this.dataStorage.getAllSolutionsForATask(this.token, res).subscribe((data) => {
            this.solutions = data;
          });
        });
      });
    }
  }
  ngOnDestroy(): void {
    if (this.getIdSubscription != null) this.getIdSubscription.unsubscribe();
    if (this.getSolutionsSubscription != null) this.getSolutionsSubscription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
  }

  downloadFile(solid: number) {
    this.dataStorage.downloadFileTask(this.token, solid);
  }
}
