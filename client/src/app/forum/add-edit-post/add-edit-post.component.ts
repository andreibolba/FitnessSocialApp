import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Toast, ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Post } from 'src/model/post.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-add-edit-post',
  templateUrl: './add-edit-post.component.html',
  styleUrls: ['./add-edit-post.component.css'],
})
export class AddEditPostComponent implements OnInit, OnDestroy {
  operation: string = 'Add';
  dataSub!: Subscription;
  sendSub!: Subscription;
  getSub!: Subscription;
  person!: Person;
  isLoading=false;

  private token: string = '';

  title!: string;
  content!: string;
  private postId:number=-1;

  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddEditPostComponent>,
    private utils:UtilsService
  ) {}

  ngOnInit(): void {
    this.isLoading=true;
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.dataSub = this.dataService.personData
        .getPerson(person.username, person.token)
        .subscribe((res) => {
          this.person = res;
          this.getSub = this.utils.postToEdit.subscribe((res)=>{
            if(res!=null){
              this.title=res.title;
              this.content=res.content;
              this.postId=res.postId;
              this.operation="Edit";
            }else{
              this.title='';
              this.content='';
              this.operation='Add';
            }
          })
        },()=>{},()=>{
          this.isLoading=false;
        });

    }
  }
  ngOnDestroy(): void {
    if (this.dataSub != null) this.dataSub.unsubscribe();
    if (this.sendSub != null) this.sendSub.unsubscribe();
    if (this.getSub != null) this.getSub.unsubscribe();
    this.utils.postToEdit.next(null);
  }

  onSignUpSubmit() {
    this.isLoading = true;
    var post = new Post();
    post.title = this.title;
    post.content = this.content;
    post.person = this.person;

    if(this.operation=="Add"){
    this.sendSub = this.dataService.forumData.postData.addPost(this.token, post).subscribe(
      (res) => {
        res.canEdit=true;
        this.dataService.forumData.postData.postAdded.next(res);
        this.toastr.success('Your post was added succefully!');
        this.dialogRef.close();
      },
      (error) => {
        this.toastr.error(error.error);
      },()=>{
        this.isLoading=false;
      }
    );
    }else{
      post.postId = this.postId;
      this.sendSub = this.dataService.forumData.postData.editPost(this.token, post).subscribe(
        (res) => {
          res.canEdit=true;
          this.dataService.forumData.postData.postAdded.next(res);
          this.toastr.success('Your post was edited succefully!');
          this.dialogRef.close();
        },
        (error) => {
          console.log(error);
          this.toastr.error(error.error);
        }
      ),()=>{
        this.isLoading=false;
      };
    }

  }
}
