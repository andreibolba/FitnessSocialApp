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
  tests: Test[] | null = null;
  private token!: string;
  testSub!: Subscription;
  trainerSub!: Subscription;
  deleteSub!: Subscription;

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
  }

  onAdd() {
    this.utils.questionToEdit.next(null);
    this.openDialog();
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditTestsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  deletetest(id: number) {
    console.log(id);
    this.deleteSub = this.dataService.deleteTest(this.token, id).subscribe(
      () => {
        this.toastr.success('Test was deleted succesfully!');
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
                  this.tests!.forEach((element) => {
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
