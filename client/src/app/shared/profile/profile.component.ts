import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { UploadPhotoComponent } from '../upload-photo/upload-photo.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  dataSub!: Subscription;
  personSub!: Subscription;
  person!: Person;
  isLoading=true;
  canEdit:boolean=false;

  constructor(private dataService: DataStorageService, private utils:UtilsService, private dialog:MatDialog, private route: Router) {}

  ngOnInit(): void {
    this.utils.initializeError();
    this.isLoading=true;
    this.utils.dashboardChanged.next(false);
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
        console.log(this.route.url);
        let idString = this.route.url.substring(this.route.url.lastIndexOf('/')+1);
        console.log(idString);
        let id =+idString;
        if(!Number.isNaN(id)){
        this.dataSub = this.dataService.personData
        .getPersonById(id, person.token)
        .subscribe(
          (res) => {
            this.person = res;
            this.canEdit=false;
          },
          (error) => {
            console.log(error.error);
          },
          ()=>{
            this.isLoading=false;
          }
        );
        }else{
          this.dataSub = this.dataService.personData
        .getPerson(person.username, person.token)
        .subscribe(
          (res) => {
            this.person = res;
            this.canEdit=true;
          },
          (error) => {
            console.log(error.error);
          },
          ()=>{
            this.isLoading=false;
          }
        );
        }
    }
  }
  ngOnDestroy(): void {
    if(this.dataSub) this.dataSub.unsubscribe();
  }

  openDialog() {
    const dialogRef =this.dialog.open(UploadPhotoComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onChangePicture(id:number){
    this.utils.idToPictureUpload.next(0);
    this.openDialog();
  }
}
