import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Post } from 'src/model/post.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditPostComponent } from '../add-edit-post/add-edit-post.component';
import { SeePostComponent } from '../see-post/see-post.component';

@Component({
  selector: 'app-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.css'],
})
export class ForumComponent implements OnInit, OnDestroy {
  date!: Date;
  token: string='';
  postsSubscription!:Subscription;
  posts:Post[]=[];

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialog
  ) {}
  ngOnDestroy(): void {
    if(this.postsSubscription) this.postsSubscription.unsubscribe();
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
      this.postsSubscription = this.dataStorage.getAllPosts(this.token).subscribe((res)=>{
        if(res!=null){
          this.posts = res;
          this.posts.forEach(element => {
            element.canEdit = element.person.username == person.username;
          });
        }
      });
    }
  }

  openDialog(op:number) {
    const dialogRef = op==1
    ? this.dialog.open(AddEditPostComponent)
    : this.dialog.open(SeePostComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.openDialog(1);
  }

  onSeePost(post: Post){
    this.openDialog(2);
  }

}
