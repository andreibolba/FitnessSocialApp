import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

export class CheckBox {
  id: number;
  name: string;
  checked: boolean;

  constructor(id: number, name: string, checked: boolean) {
    this.name = name;
    this.id = id;
    this.checked = checked;
  }
}

@Component({
  selector: 'app-edit-inters',
  templateUrl: './edit-inters.component.html',
  styleUrls: ['./edit-inters.component.css']
})
export class EditIntersComponent implements OnInit, OnDestroy {
  dataPeopleSub!: Subscription;
  testSub!: Subscription;
  utilsSub!: Subscription;
  isInternSub!: Subscription;

  test!: Test;
  options: CheckBox[] = [];
  isIntern:boolean=false;

  private token: string = '';

  constructor(
    private dataService: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditIntersComponent>
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    this.testSub = this.utils.testToEdit.subscribe((res) => {
      if(res!=null)
        this.test = res;
    });
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;

      this.isInternSub=this.utils.isInternTest.subscribe((res)=>{
        this.isIntern=res;
        if(res==true){
          this.dataPeopleSub = this.dataService.quizData.testsData
          .getAllInternsForTest(this.token, this.test!.testId)
          .subscribe((res) => {
            res.forEach((element) => {
              this.options.unshift(
                new CheckBox(
                  element.internId,
                  element.firstName + ' ' + element.lastName,
                  element.isChecked
                )
              );
            });
          });
        }else{
          this.dataPeopleSub = this.dataService.quizData.testsData
          .getAllGroupsForTest(this.token, this.test!.testId)
          .subscribe((res) => {
            res.forEach((element) => {
              this.options.unshift(
                new CheckBox(
                  element.groupId,
                  element.groupName,
                  element.isChecked
                )
              );
            });
          });
        }
      })

    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
    if (this.testSub) this.testSub.unsubscribe();
  }

  valueChange(op: CheckBox) {
    op.checked = !op.checked;
  }

  save() {
    var selected = this.options.filter((op) => op.checked == true);
    var ids: string = '';
    selected.forEach((element) => {
      ids += element.id.toString() + '_';
    });
    if(this.isIntern){
    this.dataService.quizData.testsData
      .updateAllInternsFromTest(this.token, this.test!.testId,ids)
      .subscribe(
        () => {
          this.toastr.success('Dates are update succesfully!');
          this.dialogRef.close();
        },
        (error) => {
          this.toastr.error(error.error);
          console.log(error.error);
        }
      );
    }else{
      this.dataService.quizData.testsData
      .updateAllGroupsFromTest(this.token, this.test!.testId,ids)
      .subscribe(
        () => {
          this.toastr.success('Dates are update succesfully!');
          this.dialogRef.close();
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    }
  }
}
