import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditIntersComponent } from '../edit-inters/edit-inters.component';
import { EditTestsComponent } from '../edit-tests/edit-tests.component';
import { StartTestComponent } from '../start-test/start-test.component';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css'],
})
export class TestComponent implements OnInit, OnDestroy {
  tests: Test[] = [];
  private token!: string;
  testSub!: Subscription;
  trainerSub!: Subscription;
  deleteSub!: Subscription;
  publishSub!: Subscription;
  isTrainer: boolean = false;

  constructor(
    private utils: UtilsService,
    private dialog: MatDialog,
    private dataService: DataStorageService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      let id = -1;
      this.trainerSub = this.dataService
        .getPerson(person.username, this.token)
        .subscribe(
          (data) => {
            id = data.personId;
            this.isTrainer = data.status == 'Trainer';
            this.testSub = this.dataService
              .getMyTests(this.token, id,data.status.toLocaleLowerCase())
              .subscribe(
                (data) => {
                  this.tests = data;
                },
                () => {},
                () => {
                  this.tests.forEach((element) => {
                    element.points = this.utils.calculatePoint(
                      element.questions
                    );
                  });
                }
              );
          }
        );
    }
  }

  ngOnDestroy(): void {
    if (this.testSub != null) this.testSub.unsubscribe();
    if (this.trainerSub != null) this.trainerSub.unsubscribe();
    if (this.deleteSub != null) this.deleteSub.unsubscribe();
    if (this.publishSub != null) this.publishSub.unsubscribe();
  }

  deletetest(id: number) {
    this.deleteSub = this.dataService.deleteOneTest(this.token, id).subscribe(
      () => {
        this.toastr.success('Test was deleted succesfully!');
      },
      (error) => {
        this.toastr.error(error.error);
        console.log(error);
      }
    );
  }

  publishTest(testId: number) {
    let index = this.tests.findIndex((t) => t.testId == testId);
    if (this.tests[index].questions.length == 0) {
      alert("You can't publish a test with no questions!");
      return;
    } else {
      console.log("-"+this.token+"-");
      this.publishSub = this.dataService.publish(this.token, testId).subscribe(
        () => {
          this.toastr.success('Test was published succesfully!');
          this.tests[index].canBeEdited = false;
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }

  startTest(test:Test){
    this.utils.testToStart.next(test);
    this.openDialog(3);
  }

  seeResults(test:Test){
    this.utils.testToStart.next(test);
    this.openDialog(3);
  }

  onSeeTest(test: Test) {
    this.utils.testToEdit.next(test);
    this.utils.isEditModeForTest.next(false);
    this.openDialog(1);
  }

  openDialog(op: number) {
    const dialogRef =
      op == 1
        ? this.dialog.open(EditTestsComponent)
        : op==2? this.dialog.open(EditIntersComponent)
        : this.dialog.open(StartTestComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  edittest(test: Test) {
    this.utils.testToEdit.next(test);
    this.utils.isEditModeForTest.next(true);
    this.openDialog(1);
  }

  addInterns(test: Test) {
    this.utils.testToEdit.next(test);
    this.utils.isInternTest.next(true);
    this.openDialog(2);
  }

  addGroups(test: Test) {
    this.utils.testToEdit.next(test);
    this.utils.isInternTest.next(false);
    this.openDialog(2);
  }


  onAdd() {
    this.utils.testToEdit.next(null);
    this.utils.isEditModeForTest.next(true);
    this.openDialog(1);
  }
}
