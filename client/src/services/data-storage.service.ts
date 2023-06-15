import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';
import { InternGroup } from 'src/model/interngroup.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';
import { ObjectIntern } from 'src/model/objectintern.model';
import { ObjectGroup } from 'src/model/objectgroup.model';
import { Meeting } from 'src/model/meeting.model';
import { Answer } from 'src/model/answer.model';
import { Post } from 'src/model/post.model';
import { Comment } from 'src/model/comment.model';
import { Message } from 'src/model/message.model';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { Note } from 'src/model/note.model';
import { Challenge } from 'src/model/challenge.model';
import { ChallengeSolution } from 'src/model/challengesolution.model';
import { Ranking } from 'src/model/ranking.mode';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  baseUrl = 'https://localhost:7191/api/';
  people: Person[] = [];
  constructor(private http: HttpClient) { }

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

  getPersonById(id: number, token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person>(this.baseUrl + 'people/' + id, {
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

  getGroupById(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Object>(this.baseUrl + 'group/' + groupId, { headers: headers });
  }

  addGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'group/add',
      {
        groupName: group.groupName,
        description: group.description,
        trainerId: group.trainerId,
      },
      { headers: headers }
    );
  }

  editGroup(token: string, group: Group) {
    const headers = { Authorization: 'Bearer ' + token };
    console.log(group);
    return this.http.post(
      this.baseUrl + 'group/update',
      {
        groupId: group.groupId,
        groupName: group.groupName,
        description: group.description,
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

  getAllPeopleInGroup(token: string, groupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Person[]>(
      this.baseUrl + 'group/participants/' + groupId,
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

  //tests


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
    return this.http.post(
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
    return this.http.post(this.baseUrl + 'test/stop/' + testId, {
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

  //meetings

  addMeeting(token: string, meeting: Meeting) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'meeting/add',
      {
        MeetingName: meeting.meetingName,
        MeetingLink: meeting.meetingLink,
        TrainerId: meeting.traierId,
        MeetingStartTime: meeting.meetingStartTime,
        MeetingFinishTime: meeting.meetingFinishTime,
      },
      {
        headers: headers,
      }
    );
  }

  updateMeeting(token: string, meeting: Meeting) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(
      this.baseUrl + 'meeting/update',
      {
        MeetingId: meeting.meetingId,
        MeetingName: meeting.meetingName,
        MeetingLink: meeting.meetingLink,
        TrainerId: meeting.traierId,
        MeetingStartTime: meeting.meetingStartTime,
        MeetingFinishTime: meeting.meetingFinishTime,
        InterndIds: meeting.internIds,
        GroupIds: meeting.groupIds
      },
      {
        headers: headers,
      }
    );
  }

  deleteMeeting(token: string, meetingId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'meeting/delete/' + meetingId, {
      headers: headers,
    });
  }

  getAllMeetingsForPerson(token: string, id: number, status: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Meeting[]>(this.baseUrl + 'meeting/' + id + '/' + status, {
      headers: headers,
    });
  }

  getANumberOfMeetingsForPerson(token: string, id: number, status: string, count: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Meeting[]>(this.baseUrl + 'meeting/' + id + '/' + status + '/' + count, {
      headers: headers,
    });
  }

  getAllInternsInMeeting(token: string, meetingId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectIntern[]>(
      this.baseUrl + 'meeting/all/' + meetingId + '/interns',
      {
        headers: headers,
      }
    );
  }

  getAllGroupsInMeeting(token: string, meetingId: number, trainerId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<ObjectGroup[]>(
      this.baseUrl + 'meeting/all/' + meetingId + '/groups/' + trainerId,
      {
        headers: headers,
      }
    );
  }

  //questionsolution

  sendTest(token: string, internId: number, testId: number, answers: Answer[]) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'questionsolution/add/' + internId + '/' + testId, answers, {
      headers: headers,
    });
  }

  getResult(token: string, internId: number, testId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Answer[]>(this.baseUrl + 'questionsolution/getanswers/' + internId + '/' + testId, {
      headers: headers,
    });
  }

  //posts

  getAllPostsCompleted(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Post[]>(this.baseUrl + 'posts/completed/' + personId, {
      headers: headers,
    });
  }

  addPost(token: string, post: Post) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'posts/add', {
      Title: post.title,
      Content: post.content,
      PersonId: post.person.personId
    }, { headers: headers })
  }

  editPost(token: string, post: Post) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'posts/edit', {
      PostId: post.postId,
      Title: post.title,
      Content: post.content,
      PersonId: post.person.personId
    }, { headers: headers });
  }

  addView(token: string, postId: number, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'posts/view', {
      PostId: postId,
      PersonId: personId
    }, { headers: headers });
  }

  votePost(token: string, postId: number, personId: number, upvote: boolean, downvote: boolean) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'posts/vote', {
      PostId: postId,
      PersonId: personId,
      Upvote: upvote,
      Downvote: downvote
    }, { headers: headers });
  }

  deletePost(token: string, postId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'posts/delete/' + postId, {}, { headers: headers });
  }

  //comment

  getAllCommForPostAndPerson(token: string, postId: number, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Comment[]>(this.baseUrl + 'comment/post/' + postId + '/' + personId, {
      headers: headers,
    });
  }

  voteComment(token: string, commentId: number, personId: number, upvote: boolean, downvote: boolean) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'comment/vote', {
      CommentId: commentId,
      PersonId: personId,
      Upvote: upvote,
      Downvote: downvote
    }, { headers: headers });
  }

  deleteComment(token: string, commentId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'comment/delete/' + commentId, {}, { headers: headers });
  }

  addComment(token: string, comment: Comment) {
    console.log(comment);
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'comment/add', {
      CommentContent: comment.commentContent,
      PostId: comment.postId,
      PersonId: comment.person.personId
    }, { headers: headers })
  }

  editComment(token: string, comment: Comment) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'comment/edit', {
      CommentId: comment.commentId,
      CommentContent: comment.commentContent,
      PostId: comment.postId,
      PersonId: comment.person.personId,
      DateOfComment: comment.dateOfComment
    }, { headers: headers });
  }

  //chat

  getLastMessagesFormChatForCurrentPerson(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Message[]>(this.baseUrl + 'chat/' + personId, { headers: headers });
  }

  getAllMessagesFromAChat(token: string, currentPersonId: number, chatPersonId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Message[]>(this.baseUrl + 'chat/messages/' + currentPersonId + '/' + chatPersonId, { headers: headers });
  }

  addMessage(token: string, message: Message) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Message>(this.baseUrl + 'chat/add', {
      PersonSenderId: message.personSenderId,
      PersonReceiverId: message.personReceiverId,
      Message: message.message
    }, { headers: headers });
  }

  deleteChat(token: string, personId: number, chatPersonId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'chat/deletechat/' + personId + '/' + chatPersonId, {}, { headers: headers });
  }

  //groupchat

  addGroupChat(token: string, nameOfGroup: string, descriptionOfGroup: string, adminId: number, ids: number[]) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChatMessage>(this.baseUrl + 'groupchat/add', {
      NameOfGroup: nameOfGroup,
      DescriptionOfGroup: descriptionOfGroup,
      AdminId: adminId,
      Ids: ids
    }, { headers: headers });
  }

  editGroupChat(token: string, group: GroupChat) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/edit', {
      GroupChatId: group.groupChatId,
      GroupChatName: group.groupChatName,
      GroupChatDescription: group.groupChatDescription,
      AdminId: group.adminId
    }, { headers: headers });
  }

  getAllGroupChatsLastMessages(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<GroupChatMessage[]>(this.baseUrl + 'groupchat/' + personId, { headers: headers });
  }

  getAllMessagesForGroup(token: string, geroupId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<GroupChatMessage[]>(this.baseUrl + 'groupchat/messages/' + geroupId, { headers: headers });
  }

  sendMessageToGroupChat(token: string, personId: number, groupId: number, message: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChatMessage>(this.baseUrl + 'groupchat/message/add', {
      PersonId: personId,
      GroupChatId: groupId,
      Message: message
    }, { headers: headers });
  }

  deleteGroupChat(token: string, groupChatId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/delete/' + groupChatId, {}, { headers: headers });
  }

  removeMemberFromGroupChat(token: string, groupChatId: number, memberId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/deleteperson/' + groupChatId + '/' + memberId, {}, { headers: headers });
  }

  makeAdminGroupChat(token: string, groupChatId: number, memberId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post(this.baseUrl + 'groupchat/makeadmin/' + groupChatId + '/' + memberId, {}, { headers: headers });
  }

  updateMembersGroupChat(token: string, groupChatId: number, ids: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<GroupChat>(this.baseUrl + 'groupchat/update/members/' + groupChatId, { ids: ids }, { headers: headers });
  }

  //note

  getAllNotes(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Note[]>(this.baseUrl + 'note', { headers: headers });
  }

  getNoteById(token: string, noteId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Note>(this.baseUrl + 'note/' + noteId, { headers: headers });
  }

  createNote(token: string, note: Note) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/add', {
      noteTitle: note.noteTitle,
      noteBody: note.noteBody,
      personId: note.personId
    }, { headers: headers });
  }

  updateNote(token: string, note: Note) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/edit', {
      noteId: note.noteId,
      noteTitle: note.noteTitle,
      noteBody: note.noteBody,
      persoId: note.personId
    }, { headers: headers });
  }

  deleteNote(token: string, noteId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/delete/' + noteId, {}, { headers: headers });
  }

  //challenge

  getAllChallenges(token: string, status: string) {
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

  //challengeSolution

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
    console.log(token);
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions/mine/' + personId, { headers: headers });
  }

  getSolutionsForSpecificPersonForChallenge(token: string, personId: number, challengeId:number) {
    const headers = { Authorization: 'Bearer ' + token };
    console.log(token);
    return this.http.get<ChallengeSolution[]>(this.baseUrl + 'challenge/solutions/mine/' + personId+'/'+challengeId, { headers: headers });
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
    return this.http.post<ChallengeSolution>(this.baseUrl + 'challenge/solutions/edit/'+personId+'/'+challangeId, formData, { headers: headers });
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
