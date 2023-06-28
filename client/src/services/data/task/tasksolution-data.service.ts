import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TaskSolution } from 'src/model/tasksolution.model';

@Injectable({
  providedIn: 'root',
})
export class TaskSolutionDataService {
  private baseUrl = 'https://localhost:7191/api/';

  constructor(private http: HttpClient) { }

  addeditSolution(token: string, taskSolutionId: number, taskId: number, personId: number, file: File) {
    const headers = { Authorization: 'Bearer ' + token };
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<TaskSolution>(this.baseUrl + 'tasks/addedit/solution/' + taskSolutionId + '/' + taskId + '/' + personId, formData, { headers: headers });
  }

  getAllSolutionsForATask(token: string, taskId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TaskSolution[]>(this.baseUrl + 'tasks/solutions/' + taskId, { headers: headers });
  }

  getAllSolutionsForATaskForAPerson(token: string, taskId: number, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TaskSolution>(this.baseUrl + 'tasks/solutions/' + taskId + '/' + personId, { headers: headers });
  }

  downloadFileTask(token: string, solid: number) {
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
