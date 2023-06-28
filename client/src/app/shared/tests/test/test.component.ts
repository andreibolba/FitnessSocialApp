import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditIntersComponent } from '../edit-inters/edit-inters.component';
import { EditTestsComponent } from '../edit-tests/edit-tests.component';
import { SeeAllResultsComponent } from '../see-all-results/see-all-results.component';
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
  testSubs!: Subscription;
  fromGroupSub!: Subscription;
  isTrainer: boolean = false;
  isFromGroup: boolean = false;
  mainId: string = '';
  buttonsClass: string = '';
  isInGroup:boolean=false;


  constructor(
    private utils: UtilsService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private dataService: DataStorageService,
    private toastr: ToastrService
  ) {}

  ngOnDestroy(): void {
    if (this.testSub != null) this.testSub.unsubscribe();
    if (this.trainerSub != null) this.trainerSub.unsubscribe();
    if (this.deleteSub != null) this.deleteSub.unsubscribe();
    if (this.publishSub != null) this.publishSub.unsubscribe();
    if (this.fromGroupSub != null) this.fromGroupSub.unsubscribe();
  }

  deletetest(id: number) {
    this.deleteSub = this.dataService.quizData.testsData.deleteOneTest(this.token, id).subscribe(
      () => {
        let index = this.tests.findIndex(t=>t.testId==id);
        this.tests.splice(index,1);
        this.toastr.success('Test was deleted succesfully!');
      },
      (error) => {
        this.toastr.error(error.error);
        console.log(error);
      }
    );
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
        : op==3? this.dialog.open(StartTestComponent)
        : this.dialog.open(SeeAllResultsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  seeAllResults(test:Test){
    this.utils.testToSeeAllResult.next(test);
    this.openDialog(4);
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

  publishTest(testId: number) {
    let index = this.tests.findIndex((t) => t.testId == testId);
    if (this.tests[index].questions.length == 0) {
      alert("You can't publish a test with no questions!");
      return;
    } else {
      this.publishSub = this.dataService.quizData.testsData.publish(this.token, testId).subscribe(
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

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      let id = -1;
      this.trainerSub = this.dataService.personData
        .getPerson(person.username, this.token)
        .subscribe((data) => {
          id = data.personId;
          this.isTrainer = data.status == 'Trainer';
          this.fromGroupSub = this.utils.isFromGroupDashboard.subscribe(
            (res) => {
              this.isFromGroup = res;
              if (this.isFromGroup) {
                this.mainId = 'maingroup';
                this.buttonsClass = 'buttonsgroup';
                this.isFromGroup=true;
                this.route.params.subscribe((params: Params) => {
                  let groupId = +params['id'];
                  this.testSub = this.dataService.quizData.testsData
                    .getMyTestsByGroupId(person.token, groupId)
                    .subscribe(
                      (data) => {
                        if (data != null) this.tests = data;
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
                });
              } else {
                this.mainId = 'main';
                this.buttonsClass = 'buttons';
                this.isFromGroup=false;
                this.testSub = this.dataService.quizData.testsData
                  .getMyTests(person.token, id,data.status)
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
            }
          );
        });
        this.testSubs = this.dataService.quizData.testsData.testAdded.subscribe((res)=>{
          if(res){
            let index = this.tests.findIndex(t=>t.testId==res.testId);
            if(index==-1){
              this.tests.push(res);
            }else{
              this.tests[index]=res;
            }
          }
        })
    }
  }
}
