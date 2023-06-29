import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Feedback } from 'src/model/feedback.model';
import { Task } from 'src/model/task.model';
import { TaskGroup } from 'src/model/taskgroup.model';
import { TaskIntern } from 'src/model/taskintern.model';
import { TaskSolution } from 'src/model/tasksolution.model';

@Injectable({
  providedIn: 'root',
})
export class TaskDataService {
  private baseUrl = 'https://localhost:7191/api/';

  taskAdded = new BehaviorSubject<Task | null>(null);

  constructor(private http: HttpClient) { }

  getAllTasks(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Task[]>(this.baseUrl + 'tasks', { headers: headers });
  }

  getAllTasksForPerson(token: string, status: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Task[]>(this.baseUrl + 'tasks/' + status + '/' + personId, { headers: headers });
  }
  

  addTask(token: string, task: Task) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Task>(this.baseUrl + 'tasks/add', {
      taskName: task.taskName,
      taskDescription: task.taskDescription,
      trainerId: task.trainerId
    }, { headers: headers });
  }

  editTask(token: string, task: Task) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Task>(this.baseUrl + 'tasks/edit', {
      taskId: task.taskId,
      taskName: task.taskName,
      taskDescription: task.taskDescription,
      trainerId: task.trainerId
    }, { headers: headers });
  }

  deleteTask(token: string, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'tasks/delete/' + taskId, {}, { headers: headers });
  }

  assignTask(token: string, idsIntern: string, idsGroups: string, taskid: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'tasks/assign/' + taskid,
      {
        idsIntern: idsIntern,
        idsGroups: idsGroups
      },
      {
        headers: headers,
      }
    );
  }

  getCheckGroupForTask(token: string, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TaskGroup[]>(this.baseUrl + 'tasks/checked/group/' + taskId, { headers: headers });
  }

  getCheckInternForTask(token: string, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TaskIntern[]>(this.baseUrl + 'tasks/checked/intern/' + taskId, { headers: headers });
  }

  downloadFile(token: string, solid: number) {
    const headers = { Accept: 'application/octet-stream', Authorization: 'Bearer ' + token };
    this.http.get(this.baseUrl + 'tasks/solutions/download/' + solid, { headers: headers, responseType: 'blob' }).subscribe(response => {
      this.saveFile(response);
    });
  }

  saveFile(response: any) {
    const blob = new Blob([response], { type: 'application/octet-stream' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'attempt.zip';
    link.click();
    window.URL.revokeObjectURL(url);
  }
  
}
