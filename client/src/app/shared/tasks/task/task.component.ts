import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Task } from 'src/model/task.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditTaskComponent } from '../add-edit-task/add-edit-task.component';
import { AssignTaskComponent } from '../assign-task/assign-task.component';
import { SubTask } from 'src/model/subtask.model';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit, OnDestroy {
  getPersonSubscription!:Subscription;
  getTasksSubscription!:Subscription;
  getSubTasksSubscription!:Subscription;
  deleteTaskSubscription!:Subscription;
  tasks:Task[]=[];
  subTasks:SubTask[]=[];
  private token:string='';
  
  constructor(private utils:UtilsService, private dataStorage:DataStorageService, private toastr:ToastrService, private dialog:MatDialog) {

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
        this.getTasksSubscription = this.dataStorage.getAllTasksForPerson(this.token,res.status, res.personId).subscribe((data)=>{
          this.tasks = data;
          this.getSubTasksSubscription = this.dataStorage.getAllSubTasks(this.token).subscribe((sub)=>{
            this.subTasks=sub;
          });
        });
      });
    }
  }

  ngOnDestroy(): void {
    if(this.getPersonSubscription!=null)this.getPersonSubscription.unsubscribe();
    if(this.getTasksSubscription!=null)this.getTasksSubscription.unsubscribe();
  }

  openDialog(op: number) {
    const dialogRef = op == 1 ? 
    this.dialog.open(AddEditTaskComponent) 
    : this.dialog.open(AssignTaskComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd(){
    this.utils.taskToEdit.next(null);
    this.openDialog(1);
  }

  onEdit(task:Task){
    this.utils.taskToEdit.next(task);
    this.openDialog(1);
  }

  onDelete(taskId:number){
    this.deleteTaskSubscription = this.dataStorage.deleteTask(this.token,taskId).subscribe(()=>{
      this.toastr.success("Task deleted succesfully!");
      let index = this.tasks.findIndex(t=>t.taskId == taskId);
      this.tasks.splice(index,1);
    },(error)=>{
      this.toastr.error(error.error);
    });
  }

  onAssign(task:Task){
    this.utils.taskToEdit.next(task);
    this.openDialog(2);
  }

  addSubTask(){

  }

}
