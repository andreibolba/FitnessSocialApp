import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditTestsComponent } from '../edit-tests/edit-tests.component';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css'],
})
export class TestComponent implements OnInit, OnDestroy {
  tests: Test[]=[];
  private token!: string;
  testSub!: Subscription;
  trainerSub!: Subscription;
  deleteSub!: Subscription;
  publishSub!: Subscription;
  isTrainer:boolean=false;

  constructor(
    private utils: UtilsService,
    private dialog: MatDialog,
    private dataService: DataStorageService,
    private toastr: ToastrService
  ) {}
  ngOnDestroy(): void {
    if (this.testSub != null) this.testSub.unsubscribe();
    if (this.trainerSub != null) this.trainerSub.unsubscribe();
    if (this.deleteSub != null) this.deleteSub.unsubscribe();
    if (this.publishSub != null) this.publishSub.unsubscribe();
  }

  onAdd() {
    this.utils.testToEdit.next(null);
    this.utils.isEditModeForTest.next(true);
    this.openDialog();
  }

  onSeeTest(test:Test){
    this.utils.testToEdit.next(test);
    this.utils.isEditModeForTest.next(false);
    this.openDialog();
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditTestsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  edittest(test: Test) {
    this.utils.testToEdit.next(test);
    this.utils.isEditModeForTest.next(true);
    this.openDialog();
  }

  deletetest(id: number) {
    this.deleteSub = this.dataService.deleteTest(this.token, id).subscribe(
      () => {
        this.toastr.success('Test was deleted succesfully!');
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
  }

  publishTest(testId:number){
    this.publishSub=this.dataService.publish(this.token,testId).subscribe( () => {
      this.toastr.success('Test was published succesfully!');
      let index=this.tests.findIndex(t=>t.testId==testId);
      this.tests[index].canBeEdited=false;
    },
    (error) => {
      this.toastr.error(error.error);
    }
  );
  }

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
            this.isTrainer=data.status=='Trainer';
          },
          () => {},
          () => {
            this.testSub = this.dataService
              .getMyTests(person.token, id)
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
}
