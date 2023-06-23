import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { Meeting } from 'src/model/meeting.model';
import { Person } from 'src/model/person.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from './data-storage.service';
import { Post } from 'src/model/post.model';
import { Comment } from 'src/model/comment.model';
import { Message } from 'src/model/message.model';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { Note } from 'src/model/note.model';
import { Challenge } from 'src/model/challenge.model';
import { ChallengeSolution } from 'src/model/challengesolution.model';
import { Task } from 'src/model/task.model';
import { Feedback } from 'src/model/feedback.model';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);
  userToSeeDetailst=new BehaviorSubject<string | null>(null);
  questionToEdit=new BehaviorSubject<Question | null>(null);
  testToEdit=new BehaviorSubject<Test | null>(null);
  meetingToEdit=new BehaviorSubject<Meeting | null>(null);
  testToStart=new BehaviorSubject<Test | null>(null);
  personIdForResult=new BehaviorSubject<number>(-1);
  groupToEdit=new BehaviorSubject<Group | null>(null);
  addedPerson=new BehaviorSubject<Person | null>(null);
  isEditModeForTest=new BehaviorSubject<boolean>(true);
  isInternTest=new BehaviorSubject<boolean>(true);
  isFromGroupDashboard = new BehaviorSubject<boolean>(false);
  meetingParticipants=new BehaviorSubject<Person[]|null>(null);
  testToSeeAllResult = new BehaviorSubject<Test | null>(null);
  postToEdit = new BehaviorSubject<Post | null>(null);
  commentToEdit = new BehaviorSubject<Comment | null>(null);
  postIdToComment = new BehaviorSubject<number | null>(null);
  selectChat = new BehaviorSubject<number>(-1);
  editGroupChatOption = new BehaviorSubject<number>(-1);
  chatPersonChat = new BehaviorSubject<number>(-1);
  groupChatPersonChat = new BehaviorSubject<GroupChat | null>(null);
  newGroupChatMessage = new BehaviorSubject<GroupChatMessage | null>(null);
  noteToEdit = new BehaviorSubject<Note | null>(null);
  challengeToEdit = new BehaviorSubject<Challenge | null>(null);
  challengeSolutionToEdit = new BehaviorSubject<ChallengeSolution | null>(null);
  challengeIdForSolutionsToEdit = new BehaviorSubject<number>(-1);
  pointToSolutionApprove = new BehaviorSubject<number | null>(null);
  maxpointToSolutionApprove = new BehaviorSubject<number>(-1);
  taskToEdit = new BehaviorSubject<Task | null>(null);
  taskIdToUpload = new BehaviorSubject<number>(-1);
  //0-person,1-group,2-groupchat
  idToPictureUpload = new BehaviorSubject<number>(-1);
  idOfGroupChatToPictureUpload = new BehaviorSubject<number>(-1);
  idOfGroupToPictureUpload = new BehaviorSubject<number>(-1);
  isOnChallanges=new BehaviorSubject<boolean>(true);
  feedbackToEdit = new BehaviorSubject<Feedback | null>(null);
  dashboardChanged=new EventEmitter<boolean>(true);

  error=new BehaviorSubject<{errorCode:number,errorTitle:string,errorMessage:string} | null>(null);

  constructor(private data:DataStorageService,private router:Router) { }

  initializeError(){
   this.data.testConnection().subscribe(()=>{
   },(error)=>{
      this.error.next({
        errorCode: 500,
        errorTitle: 'Internal Server Error!',
        errorMessage:' An error has occured! We apologise and we are fixing the problem! Please try again later!'
      });
      this.router.navigate(["error"]);
   });
  }

  calculatePoint(questions:Question[]){
    let result=0;
    questions.forEach(element => {
        result=result+element.points
    });
    return result;
  }
}
