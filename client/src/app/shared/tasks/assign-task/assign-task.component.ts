import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { CheckBox } from '../../meetings/edit-meeting-dialog/edit-meeting-dialog.component';
import { EditIntersComponent } from '../../tests/edit-inters/edit-inters.component';
import { Task } from 'src/model/task.model';

@Component({
  selector: 'app-assign-task',
  templateUrl: './assign-task.component.html',
  styleUrls: ['./assign-task.component.css']
})
export class AssignTaskComponent implements OnInit, OnDestroy {
  dataPeopleSub!: Subscription;
  groupPeopleSub!: Subscription;
  taskSub!: Subscription;
  assignTaskSub!: Subscription;
  utilsSub!: Subscription;
  isInternSub!: Subscription;
  task: Task = new Task();
  optionsInterns: CheckBox[] = [];
  optionsGroups: CheckBox[] = [];
  isIntern: boolean = false;

  private token: string = '';

  constructor(
    private dataService: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditIntersComponent>
  ) { }

  ngOnInit(): void {
    this.utils.initializeError();
    this.taskSub = this.utils.taskToEdit.subscribe((res) => {
      if (res != null)
        this.task = res;
    });
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;

      this.dataPeopleSub = this.dataService
        .getCheckInternForTask(this.token, this.task.taskId)
        .subscribe((res) => {
          console.log(res);
          res.forEach((element) => {
            this.optionsInterns.unshift(
              new CheckBox(
                element.internId,
                element.intern.firstName + ' ' + element.intern.lastName,
                element.isChecked
              )
            );
          });
        });
      this.groupPeopleSub = this.dataService
        .getCheckGroupForTask(this.token, this.task.taskId)
        .subscribe((res) => {
          res.forEach((element) => {
            this.optionsGroups.unshift(
              new CheckBox(
                element.groupId,
                element.group.groupName,
                element.isChecked
              )
            );
          });
        });

    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
    if (this.taskSub) this.taskSub.unsubscribe();
    if (this.groupPeopleSub) this.groupPeopleSub.unsubscribe();
  }

  valueChange(op: CheckBox) {
    op.checked = !op.checked;
  }

  save() {
    var selectedInterns = this.optionsInterns.filter((op) => op.checked == true);
    var idsInterns: string = '';
    selectedInterns.forEach((element) => {
      idsInterns += element.id.toString() + '_';
    });

    var selectedGroups = this.optionsGroups.filter((op) => op.checked == true);
    var idsGroups: string = '';
    selectedGroups.forEach((element) => {
      idsGroups += element.id.toString() + '_';
    });

    this.assignTaskSub = this.dataService.assignTask(this.token,idsInterns,idsGroups,this.task.taskId).subscribe(
        () => {
          this.toastr.success('Dates are update succesfully!');
          this.dialogRef.close();
        },
        (error) => {
          this.toastr.error(error.error);
          console.log(error.error);
        }
      );
  }

}
