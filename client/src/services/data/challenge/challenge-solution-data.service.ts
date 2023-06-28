import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ChallengeSolution } from 'src/model/challengesolution.model';


@Injectable({
  providedIn: 'root',
})
export class ChallengeSolutionDataService {
  private baseUrl = 'https://localhost:7191/api/';

  challengeAdded = new BehaviorSubject<ChallengeSolution | null>(null);

  constructor(private http: HttpClient) { }

  getSolutions(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions', { headers: headers });
  }

  getSolutionsForSpecificChallenge(token: string, challengeId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions/' + challengeId, { headers: headers });
  }

  getSolutionsForSpecificPerson(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions/mine/' + personId, { headers: headers });
  }

  getSolutionsForSpecificPersonForChallenge(token: string, personId: number, challengeId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions/mine/' + personId + '/' + challengeId, { headers: headers });
  }

  approveSolution(token: string, solutionId: number, points: number,) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'challenge/solutions/aprrove/' + solutionId + '/' + points, {}, { headers: headers });
  }

  declineSolution(token: string, solutionId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'challenge/solutions/decline/' + solutionId, {}, { headers: headers });
  }

  deleteSolution(token: string, solutionId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'challenge/solutions/delete/' + solutionId, {}, { headers: headers });
  }

  addSolution(token: string, personId: number, challangeId: number, file: File) {
    const headers = { Authorization: 'Bearer ' + token };
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<ChallengeSolution>(this.baseUrl + 'challenge/solutions/add/' + personId + '/' + challangeId, formData, { headers: headers });
  }


  editSolution(token: string, personId: number, challangeId: number, file: File) {
    const headers = { Authorization: 'Bearer ' + token };
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<ChallengeSolution>(this.baseUrl + 'challenge/solutions/edit/' + personId + '/' + challangeId, formData, { headers: headers });
  }

  downloadFile(token: string, solid: number) {
    const headers = { Accept: 'application/octet-stream', Authorization: 'Bearer ' + token };
    this.http.get(this.baseUrl + 'challenge/solutions/download/' + solid, { headers: headers, responseType: 'blob' }).subscribe(response => {
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
