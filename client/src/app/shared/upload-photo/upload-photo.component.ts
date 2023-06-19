import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.css']
})
export class UploadPhotoComponent implements OnInit, OnDestroy {
  selectedFile = null;
  text: string = 'No file selected';
  canUpload: boolean = false;
  sendPictureSubscription!: Subscription;
  GetPeronSubscription!: Subscription;
  getIdSubscription!: Subscription;
  private token: string = '';
  private groupId: number | null = null;
  private personId: number | null = null;


  constructor(
    private dataStorage: DataStorageService,
    private dialog: MatDialogRef<UploadPhotoComponent>,
    private utils: UtilsService,
    private toastr:ToastrService,
    private uitls:UtilsService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getIdSubscription = this.uitls.idToPictureUpload.subscribe((res)=>{
        if(res==0){
          this.GetPeronSubscription = this.dataStorage.getPerson(person.username,person.token).subscribe((res)=>{
            this.personId = res.personId;
            this.groupId = null;
          })
        }else{
          this.groupId = res;
          this.personId = null;
        }
      });
    }
  }
  ngOnDestroy(): void {
    this.uitls.idToPictureUpload.next(-1);
    if (this.sendPictureSubscription != null)
      this.sendPictureSubscription.unsubscribe();
  }

  onUpload() {
    const fd = new FormData();
    if (this.selectedFile) {
      if(this.groupId){
        fd.append('image', this.selectedFile, this.selectedFile['name']);
        this.sendPictureSubscription = this.dataStorage.sendPictureForGroup(this.token,fd,this.groupId).subscribe(()=>{
          this.toastr.success("Photo was uplodede succesfully");
          this.dialog.close();
        },(error)=>{
          this.toastr.error(error.error);
        });
      }else if(this.personId){
        fd.append('image', this.selectedFile, this.selectedFile['name']);
        this.sendPictureSubscription = this.dataStorage.sendPictureForPerson(this.token,fd,this.personId).subscribe(()=>{
          this.toastr.success("Photo was uplodede succesfully");
          this.dialog.close();
        },(error)=>{
          this.toastr.error(error.error);
        });

      }
    }
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile != null) {
      this.canUpload = true;
      this.text = 'Choosen file: ' + this.selectedFile['name'];
    }
  }
}