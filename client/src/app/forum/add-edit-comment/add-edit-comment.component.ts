import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditPostComponent } from '../add-edit-post/add-edit-post.component';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Comment } from 'src/model/comment.model';

@Component({
  selector: 'app-add-edit-comment',
  templateUrl: './add-edit-comment.component.html',
  styleUrls: ['./add-edit-comment.component.css'],
})
export class AddEditCommentComponent implements OnInit, OnDestroy {
  operation: string = 'Add';
  content!: string;
  addSubscription!: Subscription;
  editSubscription!: Subscription;
  commentSubscription!: Subscription;
  postIdSubscription!: Subscription;
  personSubscription!: Subscription;
  private token: string = '';
  private loggedPerson: Person = new Person();
  private commentId: number = -1;
  private postId: number = -1;
  private datefComment: Date=new Date();
  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddEditPostComponent>,
    private utils: UtilsService
  ) {}
  ngOnInit(): void {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.personSubscription = this.dataService
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          if (data != null) {
            this.loggedPerson = data;
            this.commentSubscription = this.utils.commentToEdit.subscribe(
              (res) => {
                if (res == null) {
                  this.operation = 'Add';
                  this.postIdSubscription = this.utils.postIdToComment.subscribe((id)=>{
                    if(id!=null)
                      this.postId=id;
                  });
                } else {
                  this.operation = 'Edit';
                  this.content = res.commentContent;
                  this.commentId = res.commentId;
                  this.postId = res.postId;
                  this.datefComment = res.dateOfComment;
                }
              }
            );
          }
        });
    }
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  onSignUpSubmit() {
    let comment = new Comment();
    comment.commentContent = this.content;
    comment.person = this.loggedPerson;
    comment.postId=this.postId;

    if(this.operation=='Add'){
      this.addSubscription = this.dataService.addComment(this.token, comment).subscribe((res)=>{
        this.toastr.success("Comment added succesfully!");
        this.dialogRef.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }else{
      comment.commentId = this.commentId;
      comment.dateOfComment = this.datefComment
      this.editSubscription = this.dataService.editComment(this.token, comment).subscribe((res)=>{
        this.toastr.success("Comment edited succesfully!");
        this.dialogRef.close();
      },(error)=>{
        this.toastr.error(error.error);
      });
    }
  }
}
