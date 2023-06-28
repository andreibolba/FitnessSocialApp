import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ObjectGroup } from 'src/model/objectgroup.model';
import { ObjectIntern } from 'src/model/objectintern.model';
import { Person } from 'src/model/person.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';

@Injectable({
  providedIn: 'root',
})
export class TestDataService {
  private baseUrl = 'https://localhost:7191/api/';

  testAdded = new BehaviorSubject<Test | null>(null);

  constructor(private http: HttpClient) { }

  getAllTests(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Test[]>(this.baseUrl + 'test', {
      headers: headers,
    });
  }

  getMyTests(token: string, trainerId: number, status: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Test[]>(this.baseUrl + 'test/mytest/' + trainerId + '/' + status, {
      headers: headers,
    });
  }

  getMyTestsByGroupId(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Test[]>(this.baseUrl + 'group/tests/' + groupId, {
      headers: headers,
    });
  }

  getPeopleRezolvingTest(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person[]>(this.baseUrl + 'test/results/people/' + testId, {
      headers: headers,
    });
  }

  deleteOneTest(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/delete/' + testId, {}, {
      headers: headers,
    });
  }

  addTest(token: string, test: Test) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Test>(
      this.baseUrl + 'test/add',
      {
        TestName: test.testName,
        TrainerId: test.trainerId,
        DateOfPost: test.dateOfPost,
        Deadline: test.deadline,
      },
      {
        headers: headers,
      }
    );
  }

  updateTest(token: string, test: Test) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Test>(
      this.baseUrl + 'test/update',
      {
        TestId: test.testId,
        TestName: test.testName,
        TrainerId: test.trainerId,
        DateOfPost: test.dateOfPost,
        Deadline: test.deadline,
      },
      {
        headers: headers,
      }
    );
  }

  publish(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'test/stop/' + testId, {}, {
      headers: headers,
    });
  }

  unselectedQuestions(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Question[]>(
      this.baseUrl + 'test/unselected/' + testId,
      {
        headers: headers,
      }
    );
  }

  saveSelectedQuestion(token: string, testId: number, ids: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'test/testattribution/update/' + testId + '/tests',
      { ids: ids },
      {
        headers: headers,
      }
    );
  }

  getAllInternsForTest(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectIntern[]>(
      this.baseUrl + 'test/all/' + testId + '/interns',
      {
        headers: headers,
      }
    );
  }

  getAllGroupsForTest(token: string, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectGroup[]>(
      this.baseUrl + 'test/all/' + testId + '/groups',
      {
        headers: headers,
      }
    );
  }

  updateAllInternsFromTest(token: string, testId: number, ids: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'test/testattribution/update/' + testId + '/interns',
      { ids: ids },
      {
        headers: headers,
      }
    );
  }

  updateAllGroupsFromTest(token: string, testId: number, ids: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'test/testattribution/update/' + testId + '/groups',
      { ids: ids },
      {
        headers: headers,
      }
    );
  }
  
}
