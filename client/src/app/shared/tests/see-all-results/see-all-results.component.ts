import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { StartTestComponent } from '../start-test/start-test.component';

@Component({
  selector: 'app-see-all-results',
  templateUrl: './see-all-results.component.html',
  styleUrls: ['./see-all-results.component.css'],
})
export class SeeAllResultsComponent implements OnInit, OnDestroy {
  testSub!: Subscription;
  internsSub!: Subscription;
  token: string = '';
  test!: Test;
  interns: Person[] = [];

  constructor(
    private utils: UtilsService,
    private dataService: DataStorageService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.testSub = this.utils.testToSeeAllResult.subscribe((data) => {
        if (data != null) {
          this.test = data;
          this.internsSub = this.dataService.quizData.testsData
            .getPeopleRezolvingTest(this.token, data.testId)
            .subscribe((res) => {
              this.interns = res;
            });
        }
      });
    }
  }
  ngOnDestroy(): void {
    if (this.testSub != null) this.testSub.unsubscribe();
  }

  openDialog() {
    const dialogRef =this.dialog.open(StartTestComponent);
    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  seeTestResult(personId: number) {
    this.utils.testToStart.next(this.test);
    this.utils.personIdForResult.next(personId);
    this.openDialog();
  }
}
