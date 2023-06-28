import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Feedback } from 'src/model/feedback.model';

@Injectable({
  providedIn: 'root',
})
export class FeedbackServiceData {
  private baseUrl = 'https://localhost:7191/api/';

  feedbackAdded = new BehaviorSubject<Feedback | null>(null);

  constructor(private http: HttpClient) { }

  getAllFeedbacks(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Feedback[]>(this.baseUrl + 'feedbacks', { headers: headers });
  }

  getfeedbackById(token: string, feedbacksId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Feedback>(this.baseUrl + 'feedbacks/' + feedbacksId, { headers: headers });
  }

  getAllFeedbacksForSpecificForPerson(token: string, personid: number, status: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Feedback[]>(this.baseUrl + 'feedbacks/' + personid + '/' + status, { headers: headers });
  }

  getAllFeedbacksForSpecificForPersonFirstCount(token: string, personid: number, status: string, count: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Feedback[]>(this.baseUrl + 'feedbacks/' + personid + '/' + status + '/' + count, { headers: headers });
  }

  addFeedback(token: string, feedback: Feedback) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Feedback>(this.baseUrl + 'feedbacks/add', {
      personSenderId: feedback.personSenderId,
      personReceiverId: feedback.personReceiverId,
      taskId: feedback.taskId,
      challangeId: feedback.challangeId,
      testId: feedback.testId,
      grade: feedback.grade,
      content: feedback.content
    }, { headers: headers });
  }

  editFeedback(token: string, feedback: Feedback) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Feedback>(this.baseUrl + 'feedbacks/edit', {
      feedbackId: feedback.feedbackId,
      personSenderId: feedback.personSenderId,
      personReceiverId: feedback.personReceiverId,
      taskId: feedback.taskId,
      challangeId: feedback.challangeId,
      testId: feedback.testId,
      grade: feedback.grade,
      content: feedback.content
    }, { headers: headers });
  }

  deleteFeedback(token: string, feedbackId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'feedbacks/delete/' + feedbackId, {}, { headers: headers });
  }
  
}
