import { Token } from '@angular/compiler';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Post } from 'src/model/post.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-see-post',
  templateUrl: './see-post.component.html',
  styleUrls: ['./see-post.component.css'],
})
export class SeePostComponent implements OnInit, OnDestroy {
  date: Date = new Date();
  post: Post = new Post();
  postSub!: Subscription;
  dataSub!: Subscription;
  upvoteSub!: Subscription;
  downvoteSub!: Subscription;
  person: Person = new Person();
  up: string = '';
  down: string = '';
  token: string = '';

  constructor(
    private utils: UtilsService,
    private dataService: DataStorageService
  ) {}
  ngOnInit(): void {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.dataSub = this.dataService
        .getPerson(person.username, person.token)
        .subscribe(
          (res) => {
            this.person = res;
            this.postSub = this.utils.postToEdit.subscribe((res) => {
              if (res) {
                this.post = res;
                this.up = res.upvote == true ? 'up-checked' : 'up';
                this.down = res.downvote == true ? 'down-checked' : 'down';
              }
            });
          },
          (error) => {
            console.log(error.error);
          }
        );
    }
  }
  ngOnDestroy(): void {
    if (this.postSub != null) this.postSub.unsubscribe();
    if (this.dataSub != null) this.dataSub.unsubscribe();
    this.utils.postToEdit.next(null);
  }

  upvote(post: Post) {
    if (this.up == 'up-checked') {
      this.up = 'up';
      post.karma--;
      this.upvoteSub = this.dataService
        .vote(this.token, post.postId, this.person.personId, false, false)
        .subscribe();
    } else {
      this.up = 'up-checked';
      if (this.down == 'down-checked') post.karma++;
      post.karma++;
      this.upvoteSub = this.dataService
        .vote(this.token, post.postId, this.person.personId, true, false)
        .subscribe();
    }
    this.down = 'down';
  }

  downvote(post: Post) {
    if (this.down == 'down-checked') {
      this.down = 'down';
      post.karma++;
      this.upvoteSub = this.dataService
        .vote(this.token, post.postId, this.person.personId, false, false)
        .subscribe();
    } else {
      this.down = 'down-checked';
      if (this.up == 'up-checked') post.karma--;
      post.karma--;
      this.upvoteSub = this.dataService
        .vote(this.token, post.postId, this.person.personId, false, true)
        .subscribe();
    }
    this.up = 'up';
  }
}
