import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.css'],
})
export class UploadPhotoComponent implements OnInit, OnDestroy {
  selectedFile = null;
  text: string = 'No file selected';
  canUpload: boolean = false;
  sendPictureSubscription!: Subscription;
  private token: string = '';


  constructor(
    private dataStorage: DataStorageService,
    private utils: UtilsService,
    private toastr:ToastrService
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
    }
  }
  ngOnDestroy(): void {
    if (this.sendPictureSubscription != null)
      this.sendPictureSubscription.unsubscribe();
  }

  onUpload() {
    const fd = new FormData();
    if (this.selectedFile) {
      fd.append('image', this.selectedFile, this.selectedFile['name']);
      this.sendPictureSubscription = this.dataStorage.sendPictureForGroup(this.token,fd,6).subscribe(()=>{
        this.toastr.success("Photo was uplodede succesfully");
      },(error)=>{
        this.toastr.error(error.error);
      });
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
