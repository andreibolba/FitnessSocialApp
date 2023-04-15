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
  person: Person = new Person();
  up: string = '';
  down: string = '';

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
}
