import { Component } from '@angular/core';

@Component({
  selector: 'app-upload-photo',
  templateUrl: './upload-photo.component.html',
  styleUrls: ['./upload-photo.component.css'],
})
export class UploadPhotoComponent {
  selectedFile = null;
  text: string = 'No file selected';
  canUpload: boolean = false;

  onUpload() {
    const fd = new FormData();
    if (this.selectedFile) {
      fd.append('image', this.selectedFile, this.selectedFile['name']);
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
