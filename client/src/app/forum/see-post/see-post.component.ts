import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Post } from 'src/model/post.model';
import { Comment } from 'src/model/comment.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { MatDialog } from '@angular/material/dialog';
import { AddEditCommentComponent } from '../add-edit-comment/add-edit-comment.component';
import { ToastrService } from 'ngx-toastr';

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
  commentsSub!: Subscription;
  deleteSub!: Subscription;
  person: Person = new Person();
  up: string = '';
  down: string = '';
  token: string = '';
  comments: Comment[] = [];

  constructor(
    private utils: UtilsService,
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialog: MatDialog
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
        .subscribe((p) => {
          this.person = p;
          this.postSub = this.utils.postToEdit.subscribe((res) => {
            if (res) {
              this.post = res;
              this.up = res.upvote == true ? 'up-checked' : 'up';
              this.down = res.downvote == true ? 'down-checked' : 'down';
              this.commentsSub = this.dataService
                .getAllCommForPostAndPerson(this.token, res.postId, this.person.personId)
                .subscribe((data) => {
                  if (data) {
                    this.comments = data;
                    console.log(this.comments);
                    this.comments.forEach((element) => {
                      element.up = element.upvote == true ? 'up-checked' : 'up';
                      element.down =
                        element.downvote == true ? 'down-checked' : 'down';
                      element.canEdit =
                        element.personId == this.person.personId;
                    });
                  }
                });
            }
          });
        });
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
      this.person.karma--;
      this.upvoteSub = this.dataService
        .votePost(this.token, post.postId, this.person.personId, false, false)
        .subscribe();
    } else {
      this.up = 'up-checked';
      if (this.down == 'down-checked') {
        post.karma++;
        this.person.karma++;
      }
      post.karma++;
      this.person.karma++;
      this.upvoteSub = this.dataService
        .votePost(this.token, post.postId, this.person.personId, true, false)
        .subscribe();
    }
    this.down = 'down';
  }

  downvote(post: Post) {
    if (this.down == 'down-checked') {
      this.down = 'down';
      post.karma++;
      this.person.karma++;
      this.upvoteSub = this.dataService
        .votePost(this.token, post.postId, this.person.personId, false, false)
        .subscribe();
    } else {
      this.down = 'down-checked';
      if (this.up == 'up-checked') {
        post.karma--;
        this.person.karma--;
      }
      post.karma--;
      this.person.karma--;
      this.upvoteSub = this.dataService
        .votePost(this.token, post.postId, this.person.personId, false, true)
        .subscribe();
    }
    this.up = 'up';
  }

  upvoteComment(comment: Comment) {
    if (comment.up == 'up-checked') {
      comment.up = 'up';
      comment.karma--;
      comment.person.karma--;
      this.upvoteSub = this.dataService
        .voteComment(this.token, comment.commentId, this.person.personId, false, false)
        .subscribe();
    } else {
      comment.up = 'up-checked';
      if (comment.down == 'down-checked') {
        comment.karma++;
        comment.person.karma++;
      }
      comment.karma++;
      comment.person.karma++;
      this.upvoteSub = this.dataService
        .voteComment(this.token, comment.commentId, this.person.personId, true, false)
        .subscribe();
    }
    comment.down = 'down';
  }

  downvoteComment(comment: Comment) {
    if (comment.down == 'down-checked') {
      comment.down = 'down';
      comment.karma++;
      comment.person.karma++;
      this.upvoteSub = this.dataService
        .voteComment(this.token, comment.commentId, this.person.personId, false, false)
        .subscribe();
    } else {
      comment.down = 'down-checked';
      if (comment.up == 'up-checked') {
        comment.karma--;
        comment.person.karma--;
      }
      comment.karma--;
      comment.person.karma--;
      this.upvoteSub = this.dataService
        .voteComment(this.token, comment.commentId, this.person.personId, false, true)
        .subscribe();
    }
    comment.up = 'up';
  }

  openDialog() {
    const dialogRef =this.dialog.open(AddEditCommentComponent);
    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAddAnswer(postId:number){
    this.utils.postIdToComment.next(postId);
    this.utils.commentToEdit.next(null);
    this.openDialog();
  }

  onEdit(comment:Comment){
    this.utils.commentToEdit.next(comment);
    this.openDialog();
  }

  onDelete(commentId:number){
    this.deleteSub = this.dataService.deleteComment(this.token, commentId).subscribe((res)=>{
      this.toastr.success("Comment deleted succesfully");
    },(error)=>{
      this.toastr.error(error.error);
    })
  }
}
