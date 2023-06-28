import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Person } from 'src/model/person.model';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { PersonDataService } from './data/person-data.service';
import { GroupDataService } from './data/group-data.service';
import { NoteDataService } from './data/note-data.service';
import { ForumDataService } from './data/forum/forum-data.service';
import { MeetingDataService } from './data/meeting-data.service';
import { FeedbackServiceData } from './data/feedback-data.service';
import { ChallengeSolutionDataService } from './data/challenge/challenge-solution-data.service';
import { ChallengeDataService } from './data/challenge/challenge-data.service';
import { QuizDataService } from './data/quiz/quiz-data.service';
import { TaskDataService } from './data/task/task-data.service';
import { SubTaskDataService } from './data/task/subtask-data.service';
import { TaskSolutionDataService } from './data/task/tasksolution-data.service';
import { ChatDataService } from './data/chat/chat-data.service';
import { GroupChatDataService } from './data/chat/group-chat-data.service';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  baseUrl = 'https://localhost:7191/api/';
  hubUrl = 'https://localhost:7191/hubs/';
  people: Person[] = [];

 

  constructor(private http: HttpClient, 
    public personData:PersonDataService,
    public groupData:GroupDataService,
    public noteData:NoteDataService,
    public meetingData:MeetingDataService,
    public feedbackData:FeedbackServiceData,
    public challengeData:ChallengeDataService,
    public challengeSolutionData:ChallengeSolutionDataService,
    public quizData:QuizDataService,
    public taskData:TaskDataService,
    public subTaskData:SubTaskDataService,
    public taskSolutionData:TaskSolutionDataService,
    public chatData:ChatDataService,
    public groupChatData:GroupChatDataService,
    public forumData:ForumDataService) { }

  testConnection() {
    return this.http.get(this.baseUrl + 'serverconnection/test');
  }

  //log
  log(token: string, log: any) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'logging/log', { logDto: log }, { headers: headers });
  }

}
