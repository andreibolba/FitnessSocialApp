import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SubTask } from 'src/model/subtask.model';

@Injectable({
  providedIn: 'root',
})
export class SubTaskDataService {
  private baseUrl = 'https://localhost:7191/api/';

  constructor(private http: HttpClient) { }

  getAllSubTasks(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<SubTask[]>(this.baseUrl + 'subtasks', { headers: headers });
  }

  getAllSubtasksForTask(token: string, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<SubTask[]>(this.baseUrl + 'subtasks/' + taskId, { headers: headers });
  }

  addSubTask(token: string, subTask: SubTask) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<SubTask>(this.baseUrl + 'subtasks/add', {
      subTaskNAme: subTask.subTaskName,
      taskId: subTask.taskId
    }, { headers: headers });
  }

  editSubTask(token: string, subTask: SubTask) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<SubTask>(this.baseUrl + 'subtasks/edit', {
      subTaskId: subTask.subTaskId,
      subTaskNAme: subTask.subTaskName,
      taskId: subTask.taskId
    }, { headers: headers });
  }

  deleteSubTask(token: string, subTaskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'subtasks/delete/' + subTaskId, {}, { headers: headers });
  }

  checkSubTask(token: string, subTaskId: number, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'subtasks/check' + subTaskId + '/' + taskId, {}, { headers: headers });
  }
  
}
