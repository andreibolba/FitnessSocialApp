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

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);
  questionToEdit=new BehaviorSubject<Question | null>(null);
  testToEdit=new BehaviorSubject<Test | null>(null);
  meetingToEdit=new BehaviorSubject<Meeting | null>(null);
  testToStart=new BehaviorSubject<Test | null>(null);
  personIdForResult=new BehaviorSubject<number>(-1);
  groupToEdit=new BehaviorSubject<Group | null>(null);
  addedPerson=new BehaviorSubject<Person | null>(null);
  isEditModeForTest=new BehaviorSubject<boolean>(true);
  isInternTest=new BehaviorSubject<boolean>(true);
  isFromGroupDashboard = new BehaviorSubject<boolean>(true);
  meetingParticipants=new BehaviorSubject<Person[]|null>(null);
  testToSeeAllResult = new BehaviorSubject<Test | null>(null);
  postToEdit = new BehaviorSubject<Post | null>(null);
  commentToEdit = new BehaviorSubject<Comment | null>(null);
  postIdToComment = new BehaviorSubject<number | null>(null);
  selectChat = new BehaviorSubject<number>(-1);
  chatPersonChat = new BehaviorSubject<number>(-1);
  groupChatPersonChat = new BehaviorSubject<GroupChat | null>(null);
  newChat = new BehaviorSubject<Message | null>(null);
  newGroupChatMessage = new BehaviorSubject<GroupChatMessage | null>(null);
  dashboardChanged=new EventEmitter<boolean>(true);

  error=new BehaviorSubject<{errorCode:number,errorTitle:string,errorMessage:string} | null>(null);

  constructor(private data:DataStorageService,private router:Router) { }

  initializeError(){
   this.data.testConnection().subscribe(()=>{
   },()=>{
      this.error.next({
        errorCode: 500,
        errorTitle: 'Internal Server Error!',
        errorMessage:' An error has occured! We apologise and we are fixing the problem! Please try again later!'
      });
      this.router.navigate(["error"]);
   })
  }

  calculatePoint(questions:Question[]){
    let result=0;
    questions.forEach(element => {
        result=result+element.points
    });
    return result;
  }
}
