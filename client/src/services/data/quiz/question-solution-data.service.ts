import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Answer } from 'src/model/answer.model';
import { Feedback } from 'src/model/feedback.model';

@Injectable({
  providedIn: 'root',
})
export class QuestionSolutionDataService {
  private baseUrl = 'https://localhost:7191/api/';

  constructor(private http: HttpClient) { }

  sendTest(token: string, internId: number, testId: number, answers: Answer[]) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'questionsolution/add/' + internId + '/' + testId, answers, {
      headers: headers,
    });
  }

  getResult(token: string, internId: number, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Answer[]>(this.baseUrl + 'questionsolution/getanswers/' + internId + '/' + testId, {
      headers: headers,
    });
  }
  
}
