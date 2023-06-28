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
import { UploadTaskSolutionComponent } from '../upload-task-solution/upload-task-solution.component';
import { SeeAllTaskSolutionsComponent } from '../see-all-task-solutions/see-all-task-solutions.component';
import { TaskSolution } from 'src/model/tasksolution.model';
import { ActivatedRoute, Params, Router } from '@angular/router';

export class SubTaskClient {
  subTask: SubTask = new SubTask();
  editMode: boolean = false;
}

export class TaskClient {
  task: Task = new Task();
  solution: TaskSolution = new TaskSolution();
  subTasks: SubTaskClient[] = [];
}

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})

export class TaskComponent implements OnInit, OnDestroy {
  getPersonSubscription!: Subscription;
  getTasksSubscription!: Subscription;
  getSubTasksSubscription!: Subscription;
  getSolutionTaskSubscription!: Subscription;
  deleteTaskSubscription!: Subscription;
  deleteSubTaskSubscription!: Subscription;
  addSubTaskSubscription!: Subscription;
  taskSubscription!: Subscription;
  fromGroupSubscription!: Subscription;
  tasks: TaskClient[] = [];
  editMode: boolean = false;
  isFromGroup: boolean = false;
  panelOpenState = false;
  private token: string = '';
  buttonsClass: string = '';
  mainId: string = '';

  constructor(private utils: UtilsService, private dataStorage: DataStorageService, private toastr: ToastrService, private dialog: MatDialog, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username, person.token).subscribe((res) => {
        this.editMode = res.status == "Trainer";
        this.fromGroupSubscription = this.utils.isFromGroupDashboard.subscribe(
          (data) => {
            this.isFromGroup = data || !this.router.url.endsWith('tests');
            if (this.isFromGroup) {
              this.mainId = 'maingroup';
              this.buttonsClass = 'buttonsgroup';
              this.isFromGroup = true;
              this.route.params.subscribe((params: Params) => {
                let groupId = +params['id'];
                this.getTasksSubscription = this.dataStorage.taskData.getAllTasksForPerson(this.token, 'group', groupId).subscribe((data) => {
                  data.forEach(element => {
                    let taskCl = new TaskClient();
                    taskCl.task = element;
                    this.getSubTasksSubscription = this.dataStorage.subTaskData.getAllSubtasksForTask(this.token, element.taskId).subscribe((sub) => {
                      sub.forEach(element => {
                        let cl = new SubTaskClient();
                        cl.subTask = element;
                        cl.editMode = false;
                        taskCl.subTasks.push(cl);
                      });
                    });
                    this.getSolutionTaskSubscription = this.dataStorage.taskSolutionData.getAllSolutionsForATaskForAPerson(this.token, element.taskId, res.personId).subscribe((sol) => {
                      taskCl.solution = sol == null ? new TaskSolution() : sol;
                    });
                    this.tasks.push(taskCl);
                  });
                });
              });
            } else {
              this.mainId = 'main';
              this.buttonsClass = 'buttons';
              this.isFromGroup = false;
              this.getTasksSubscription = this.dataStorage.taskData.getAllTasksForPerson(this.token, res.status.toLocaleLowerCase(), res.personId).subscribe((data) => {
                data.forEach(element => {
                  let taskCl = new TaskClient();
                  taskCl.task = element;
                  this.getSubTasksSubscription = this.dataStorage.subTaskData.getAllSubtasksForTask(this.token, element.taskId).subscribe((sub) => {
                    sub.forEach(element => {
                      let cl = new SubTaskClient();
                      cl.subTask = element;
                      cl.editMode = false;
                      taskCl.subTasks.push(cl);
                    });
                  });
                  this.getSolutionTaskSubscription = this.dataStorage.taskSolutionData.getAllSolutionsForATaskForAPerson(this.token, element.taskId, res.personId).subscribe((sol) => {
                    taskCl.solution = sol == null ? new TaskSolution() : sol;
                  });
                  this.tasks.push(taskCl);
                });
              });
            }
          }
        );
      });

      this.taskSubscription = this.dataStorage.taskData.taskAdded.subscribe((res) => {
        if (res) {
          let index = this.tasks.findIndex(t => t.task.taskId == res.taskId);
          if (index == -1) {
            let task = new TaskClient();
            task.task = res;
            this.tasks.unshift(task);
          } else {
            this.tasks[index].task = res;
          }
        }
      })
    }
  }

  ngOnDestroy(): void {
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.getTasksSubscription != null) this.getTasksSubscription.unsubscribe();
    if (this.getSubTasksSubscription != null) this.getSubTasksSubscription.unsubscribe();
    if (this.deleteTaskSubscription != null) this.deleteTaskSubscription.unsubscribe();
    if (this.addSubTaskSubscription != null) this.addSubTaskSubscription.unsubscribe();
    if (this.deleteSubTaskSubscription != null) this.deleteSubTaskSubscription.unsubscribe();
  }

  openDialog(op: number) {
    const dialogRef = op == 1 ?
      this.dialog.open(AddEditTaskComponent)
      : op == 2 ? this.dialog.open(AssignTaskComponent)
        : op == 3 ? this.dialog.open(UploadTaskSolutionComponent)
          : this.dialog.open(SeeAllTaskSolutionsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.taskToEdit.next(null);
    this.openDialog(1);
  }

  onEdit(task: Task) {
    this.utils.taskToEdit.next(task);
    this.openDialog(1);
  }

  onDelete(taskId: number) {
    this.deleteTaskSubscription = this.dataStorage.taskData.deleteTask(this.token, taskId).subscribe(() => {
      this.toastr.success("Task deleted succesfully!");
      let index = this.tasks.findIndex(t => t.task.taskId == taskId);
      this.tasks.splice(index, 1);
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  onAssign(task: Task) {
    this.utils.taskToEdit.next(task);
    this.openDialog(2);
  }

  onUpload(task: Task) {
    this.utils.taskIdToUpload.next(task.taskId);
    this.openDialog(3);
  }

  onAddSubTask(taskId: number) {
    let subtask = new SubTaskClient();
    subtask.editMode = true;

    let index = this.tasks.findIndex(t => t.task.taskId == taskId);
    this.tasks[index].subTasks.push(subtask);
  }

  onSaveSubTask(subtask: SubTaskClient, taskId: number) {
    if (subtask.subTask.subTaskName == '') {
      this.toastr.error("Subtask must have a name!");
      return;
    }

    subtask.subTask.taskId = taskId;

    if (subtask.subTask.subTaskId == -1) {
      this.addSubTaskSubscription = this.dataStorage.subTaskData.addSubTask(this.token, subtask.subTask).subscribe((res) => {
        this.toastr.success("SubTask added succesfully!");
        subtask.subTask = res;
        subtask.editMode = false;
      }, (error) => {
        this.toastr.error(error.error);
      });
    } else {
      console.log("edit");
      this.addSubTaskSubscription = this.dataStorage.subTaskData.editSubTask(this.token, subtask.subTask).subscribe((res) => {
        this.toastr.success("SubTask edited succesfully!");
        subtask.subTask = res;
        subtask.editMode = false;
      }, (error) => {
        this.toastr.error(error.error);
      });
    }
  }

  onEditSubTask(subtask: SubTaskClient) {
    subtask.editMode = true;
  }

  onDeleteSubTask(subtask: SubTaskClient) {
    this.deleteSubTaskSubscription = this.dataStorage.subTaskData.deleteSubTask(this.token, subtask.subTask.subTaskId).subscribe(() => {
      this.toastr.success("SubTask deleted succesfully!");
      let index = this.tasks.findIndex(t => t.task.taskId == subtask.subTask.taskId);
      let indexSubTask = this.tasks[index].subTasks.findIndex(t => t.subTask.subTaskId == subtask.subTask.subTaskId);
      this.tasks[index].subTasks.splice(indexSubTask, 1);
    }, (error) => {
      this.toastr.error(error.error);
    });
  }

  onSeeSolutions(task: Task) {
    this.utils.taskIdToUpload.next(task.taskId);
    this.openDialog(4);
  }

  onDownload(task: Task) {

  }

}
