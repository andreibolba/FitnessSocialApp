import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Comment } from 'src/model/comment.model';

@Injectable({
  providedIn: 'root',
})
export class CommentDataService {
  private baseUrl = 'https://localhost:7191/api/';

  commentAdded = new BehaviorSubject<Comment | null>(null);

  constructor(private http: HttpClient) { }

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
    return this.http.post<Comment>(this.baseUrl + 'comment/add', {
      CommentContent: comment.commentContent,
      PostId: comment.postId,
      PersonId: comment.person.personId
    }, { headers: headers })
  }

  editComment(token: string, comment: Comment) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Comment>(this.baseUrl + 'comment/edit', {
      CommentId: comment.commentId,
      CommentContent: comment.commentContent,
      PostId: comment.postId,
      PersonId: comment.person.personId,
      DateOfComment: comment.dateOfComment
    }, { headers: headers });
  }

  
}
