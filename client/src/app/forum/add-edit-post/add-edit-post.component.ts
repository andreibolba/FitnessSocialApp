import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Toast, ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { Post } from 'src/model/post.model';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-add-edit-post',
  templateUrl: './add-edit-post.component.html',
  styleUrls: ['./add-edit-post.component.css'],
})
export class AddEditPostComponent implements OnInit, OnDestroy {
  operation: string = 'Add';
  dataSub!: Subscription;
  sendSub!: Subscription;
  person!: Person;
  private token: string = '';

  title!: string;
  content!: string;

  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddEditPostComponent>
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
        .subscribe((res) => {
          this.person = res;
        });
    }
  }
  ngOnDestroy(): void {
    if (this.dataSub != null) this.dataSub.unsubscribe();
  }

  onSignUpSubmit() {
    var post = new Post();
    post.title = this.title;
    post.content = this.content;
    post.person = this.person;

    console.log(post);

    this.sendSub = this.dataService.addPost(this.token, post).subscribe(
      () => {
        this.toastr.success('Your post was added succefully!');
        this.dialogRef.close();
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
  }
}
