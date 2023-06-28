import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Question } from 'src/model/question.model';

@Injectable({
  providedIn: 'root',
})
export class QuestionDataService {
  private baseUrl = 'https://localhost:7191/api/';

  questionAdded = new BehaviorSubject<Question | null>(null);

  constructor(private http: HttpClient) { }

  getAllQuestionsByTrainer(token: string, trainerId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Question[]>(
      this.baseUrl + 'question/trainers/' + trainerId,
      {
        headers: headers,
      }
    );
  }

  addQuestion(token: string, question: Question) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Question>(
      this.baseUrl + 'question/add',
      {
        QuestionName: question.questionName,
        TrainerId: question.trainerId,
        A: question.a,
        B: question.b,
        C: question.c,
        D: question.d,
        E: question.e,
        F: question.f,
        CorrectOption: question.correctOption,
        Points: question.points,
      },
      {
        headers: headers,
      }
    );
  }

  editQuestion(token: string, question: Question) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Question>(
      this.baseUrl + 'question/update',
      {
        QuestionId: question.questionId,
        QuestionName: question.questionName,
        TrainerId: question.trainerId,
        A: question.a,
        B: question.b,
        C: question.c,
        D: question.d,
        E: question.e,
        F: question.f,
        CorrectOption: question.correctOption,
        Points: question.points,
      },
      {
        headers: headers,
      }
    );
  }

  deleteQuestion(token: string, questionId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'question/delete/' + questionId, {}, {
      headers: headers,
    });
  }
 
}
