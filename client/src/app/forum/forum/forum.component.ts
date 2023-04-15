import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Post } from 'src/model/post.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditPostComponent } from '../add-edit-post/add-edit-post.component';
import { SeePostComponent } from '../see-post/see-post.component';
import { Person } from 'src/model/person.model';

@Component({
  selector: 'app-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.css'],
})
export class ForumComponent implements OnInit, OnDestroy {
  date!: Date;
  token: string = '';
  postsSubscription!: Subscription;
  viewSubscription!: Subscription;
  personSubscription!: Subscription;
  person: Person = new Person();
  posts: Post[] = [];

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialog
  ) {}
  ngOnDestroy(): void {
    if (this.postsSubscription) this.postsSubscription.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    this.date = new Date();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.personSubscription = this.dataStorage
        .getPerson(person.username, person.token)
        .subscribe((res) => {
          if (res) this.person = res;
          this.postsSubscription = this.dataStorage
            .getAllPostsCompleted(this.token,res.personId)
            .subscribe((res) => {
              if (res != null) {
                this.posts = res;
                this.posts.forEach((element) => {
                  element.canEdit = element.person.username == person.username;
                });
              }
            });
        });
    }
  }

  openDialog(op: number) {
    const dialogRef =
      op == 1
        ? this.dialog.open(AddEditPostComponent)
        : this.dialog.open(SeePostComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.openDialog(1);
  }

  onSeePost(post: Post) {
    this.viewSubscription = this.dataStorage
      .addView(this.token, post.postId, this.person.personId)
      .subscribe(
        (res) => {
          if (res != null) post.views++;
        },
        () => {},
        () => {
          this.utils.postToEdit.next(post);
          this.openDialog(2);
        }
      );
  }

  onEdit(post: Post) {
    this.utils.postToEdit.next(post);
    this.openDialog(1);
  }
}
