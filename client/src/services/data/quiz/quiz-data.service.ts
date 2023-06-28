import { Injectable } from '@angular/core';
import { TestDataService } from './test-data.service';
import { QuestionDataService } from './question-data.service';
import { QuestionSolutionDataService } from './question-solution-data.service';

@Injectable({
  providedIn: 'root',
})
export class QuizDataService {
  constructor(public questionData:QuestionDataService, public testsData:TestDataService, public questionSolutionData:QuestionSolutionDataService) { }
  
}
