import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Challenge } from 'src/model/challenge.model';
import { Ranking } from 'src/model/ranking.mode';


@Injectable({
  providedIn: 'root',
})
export class ChallengeDataService {
  private baseUrl = 'https://localhost:7191/api/';

  challengeAdded = new BehaviorSubject<Challenge | null>(null);

  constructor(private http: HttpClient) { }

  getAllChallenges(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Challenge[]>(this.baseUrl + 'challenge', { headers: headers });
  }

  getAllChallengesForPeople(token: string, status: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return status == "Trainer" ? this.http.get<Challenge[]>(this.baseUrl + 'challenge', { headers: headers }) : this.http.get<Challenge[]>(this.baseUrl + 'challenge/intern', { headers: headers });
  }

  addChallenge(token: string, challenge: Challenge) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Challenge>(this.baseUrl + 'challenge/add', {
      challangeName: challenge.challangeName,
      challangeDescription: challenge.challangeDescription,
      trainerId: challenge.trainerId,
      deadline: challenge.deadline,
      points: challenge.points
    }, { headers: headers });
  }

  editChallenge(token: string, challenge: Challenge) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Challenge>(this.baseUrl + 'challenge/edit', {
      challangeId: challenge.challangeId,
      challangeName: challenge.challangeName,
      challangeDescription: challenge.challangeDescription,
      trainerId: challenge.trainerId,
      deadline: challenge.deadline,
      points: challenge.points
    }, { headers: headers });
  }

  deleteChallenge(token: string, challengeId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'challenge/delete/' + challengeId, {}, { headers: headers });
  }

  rankings(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Ranking[]>(this.baseUrl + 'challenge/ranking', { headers: headers });
  }
  
}
