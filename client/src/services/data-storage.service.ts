import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';
import { InternGroup } from 'src/model/interngroup.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';
import { TestIntern } from 'src/model/testintern.model';
import { TestGroup } from 'src/model/testgroup.model';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  baseUrl = 'https://localhost:7191/api/';
  people: Person[] = [];
  constructor(private http: HttpClient) {}

  //connection test

  testConnection() {
    return this.http.get(this.baseUrl + 'serverconnection/test');
  }

  //person CRUD
  getPerson(username: string, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person>(this.baseUrl + 'people/' + username, {
      headers: headers,
    });
  }

  getPeople(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person[]>(this.baseUrl + 'people', {
      headers: headers,
    });
  }

  deletePerson(personId: number, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'people/delete/' + personId,
      {},
      {
        headers: headers,
      }
    );
  }

  addperson(person: Person, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'account/register',
      {
        FirstName: person.firstName,
        LastName: person.lastName,
        Email: person.email,
        Username: person.username,
        Status: person.status,
        BirthDate: person.birthDate,
        Created: new Date(),
      },
      { headers: headers }
    );
  }

  editperson(person: Person, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'people/update',
      {
        PersonId: person.personId,
        FirstName: person.firstName,
        LastName: person.lastName,
        Email: person.email,
        Username: person.username,
        Status: person.status,
        BirthDate: person.birthDate,
      },
      { headers: headers }
    );
  }

  sendEmail(email: string) {
    return this.http.post(this.baseUrl + 'people/forgot', {
      email: email,
      time: new Date(),
    });
  }

  isLinkValid(linkid: number) {
    return this.http.post(this.baseUrl + 'people/link', {
      linkid: linkid,
      time: new Date(),
    });
  }

  resetPassword(linkId: number, password: string) {
    return this.http.post(this.baseUrl + 'people/reset', {
      linkid: linkId,
      password: password,
    });
  }

  //groups CRUD
  getGroups(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Group[]>(this.baseUrl + 'group', { headers: headers });
  }

  addGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'group/add',
      {
        groupName: group.groupName,
        trainerId: group.trainerId,
      },
      { headers: headers }
    );
  }

  editGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'group/update',
      {
        groupId: group.groupId,
        name: group.groupName,
        trainerId: group.trainerId,
      },
      { headers: headers }
    );
  }

  deleteGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'group/delete/' + groupId,
      {},
      {
        headers: headers,
      }
    );
  }

  getAllInternsInGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<InternGroup[]>(
      this.baseUrl + 'interngroup/interns/' + groupId,
      {
        headers: headers,
      }
    );
  }

  updateInternInGroup(token: string, ids: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    console.log(ids);
    return this.http.post(
      this.baseUrl + 'interngroup/interns/update/' + groupId,
      {
        ids: ids,
      },
      {
        headers: headers,
      }
    );
  }

  //questions

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
    return this.http.post(
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
    return this.http.post(
      this.baseUrl + 'question/update',
      {
        QuestionId:question.questionId,
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
    return this.http.post(this.baseUrl + 'question/delete/' + questionId, {
      headers: headers,
    });
  }

  //tests

  getMyTests(token:string,trainerId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Test[]>(this.baseUrl + 'test/mytest/' + trainerId, {
      headers: headers,
    });
  }

  deleteTest(token:string, testId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/delete/' + testId, {
      headers: headers,
    });
  }

  addTest(token:string, test:Test){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Test>(this.baseUrl + 'test/add',{
      TestName: test.testName,
      TrainerId: test.trainerId,
      DateOfPost:test.dateOfPost,
      Deadline:test.deadline,
    }, {
      headers: headers,
    });
  }

  updateTest(token:string, test:Test){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/update',{
      TestId: test.testId,
      TestName: test.testName,
      TrainerId: test.trainerId,
      DateOfPost:test.dateOfPost,
      Deadline:test.deadline,
    }, {
      headers: headers,
    });
  }

  publish(token:string, testId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/stop/'+testId, {
      headers: headers,
    });
  }

  unselectedQuestions(token:string, testId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Question[]>(this.baseUrl + 'test/unselected/'+testId, {
      headers: headers,
    });
  }

  saveSelectedQuestion(token:string, testId:number,ids:string){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/testattribution/update/'+testId+'/tests',{ids: ids}, {
      headers: headers,
    });
  }

  getAllInternsForTest(token:string, testId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TestIntern[]>(this.baseUrl + 'test/all/'+testId+'/interns', {
      headers: headers,
    });
  }

  getAllGroupsForTest(token:string, testId:number){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<TestGroup[]>(this.baseUrl + 'test/all/'+testId+'/groups', {
      headers: headers,
    });
  }

  updateAllInternsFromTest(token:string, testId:number,ids:string){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/testattribution/update/'+testId+'/interns',{ids: ids}, {
      headers: headers,
    });
  }

  updateAllGroupsFromTest(token:string, testId:number, ids:string){
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/testattribution/update/'+testId+'/groups',{ids: ids}, {
      headers: headers,
    });
  }
}
