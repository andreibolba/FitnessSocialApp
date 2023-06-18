import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Task } from 'src/model/task.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-add-edit-task',
  templateUrl: './add-edit-task.component.html',
  styleUrls: ['./add-edit-task.component.css']
})
export class AddEditTaskComponent implements OnInit, OnDestroy {
  getPersonSubscription!:Subscription;
  getTaskSubscription!:Subscription;
  addEditTaskSubscription!:Subscription;
  task:Task=new Task();
  taskName:string='';
  taskDescription:string='';
  isEdit:boolean=false;
  operation:string='';
  private token:string='';
  private trainerId:number=-1;
  
  constructor(private utils:UtilsService, private dataStorage:DataStorageService, private toastr:ToastrService, private dialog:MatDialogRef<AddEditTaskComponent>) {

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
        this.trainerId=res.personId;
        this.getTaskSubscription = this.utils.taskToEdit.subscribe((data)=>{
          if(data){
            this.task=data;
            this.taskName=data.taskName;
            this.taskDescription=data.taskDescription;
            this.isEdit=true;
            this.operation="Edit";
          }else{
            this.isEdit=false;
            this.operation="Add";
          }
        })
      });
    }
  }

  ngOnDestroy(): void {
    if(this.getPersonSubscription!=null)this.getPersonSubscription.unsubscribe();
    if(this.addEditTaskSubscription!=null)this.addEditTaskSubscription.unsubscribe();
    if(this.getTaskSubscription!=null)this.getTaskSubscription.unsubscribe();
  }

  onSubmit(){
    let task=new Task();
    task.taskName=this.taskName;
    task.taskDescription=this.taskDescription;
    task.trainerId=this.trainerId;
    if(!this.isEdit){
      this.addEditTaskSubscription=this.dataStorage.addTask(this.token, task).subscribe(()=>{
        this.toastr.success("Task added successfully");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }else{
      task.taskId=this.task.taskId;
      this.addEditTaskSubscription=this.dataStorage.editTask(this.token, task).subscribe(()=>{
        this.toastr.success("Task added successfully");
        this.dialog.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }
  }
}
