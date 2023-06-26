import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditSolutionComponent } from '../../challenge/add-edit-solution/add-edit-solution.component';

@Component({
  selector: 'app-upload-task-solution',
  templateUrl: './upload-task-solution.component.html',
  styleUrls: ['./upload-task-solution.component.css']
})
export class UploadTaskSolutionComponent {
  selectedFile = null;
  text: string = 'No file selected';
  hasSolution: boolean = false;
  canUpload: boolean = false;
  sendFileSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  getSolutionSubscription!: Subscription;
  getTaskIdSubscription!: Subscription;
  person: Person = new Person();
  private token: string = '';
  private taskId: number = -1;
  private taskSolutionId: number = -1;


constructor(
    private dataStorage: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialog: MatDialogRef<AddEditSolutionComponent>
  ) { }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubscription = this.dataStorage.personData.getPerson(person.username, this.token).subscribe((res) => {
        this.person = res;
        this.getTaskIdSubscription = this.utils.taskIdToUpload.subscribe((id) => {
          this.taskId=id;
        });
      });
    }
  }
  ngOnDestroy(): void {
    if (this.sendFileSubscription != null) this.sendFileSubscription.unsubscribe();
    if (this.getSolutionSubscription != null) this.getSolutionSubscription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.getTaskIdSubscription != null) this.getTaskIdSubscription.unsubscribe();
  }

  onUpload() {
    if (this.selectedFile) {
      if(this.hasSolution){
        this.sendFileSubscription = this.dataStorage.addeditSolution(this.token, this.taskSolutionId, -1, -1, this.selectedFile).subscribe((res) => {
          this.toastr.success("Solution uploaded succesfully");
          this.dialog.close();
        },(error)=>{
          this.toastr.error(error.error);
        });
      }else{
        this.sendFileSubscription = this.dataStorage.addeditSolution(this.token, -1, this.taskId, this.person.personId, this.selectedFile).subscribe((res) => {
          this.toastr.success("Solution added succesfully");
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
