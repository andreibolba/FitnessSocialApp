import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Post } from 'src/model/post.model';


@Injectable({
  providedIn: 'root',
})
export class PostDataService {
  private baseUrl = 'https://localhost:7191/api/';

  postAdded = new BehaviorSubject<Post | null>(null);

  constructor(private http: HttpClient) { }

  getAllPosts(token: string, personId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Post[]>(this.baseUrl + 'posts', {
      headers: headers,
    });
  }

  addPost(token: string, post: Post) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Post>(this.baseUrl + 'posts/add', {
      Title: post.title,
      Content: post.content,
      PersonId: post.person.personId
    }, { headers: headers })
  }

  editPost(token: string, post: Post) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Post>(this.baseUrl + 'posts/edit', {
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

}
