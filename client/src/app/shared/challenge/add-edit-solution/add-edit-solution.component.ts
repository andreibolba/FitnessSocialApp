import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-add-edit-solution',
  templateUrl: './add-edit-solution.component.html',
  styleUrls: ['./add-edit-solution.component.css']
})
export class AddEditSolutionComponent implements OnInit, OnDestroy {
  selectedFile = null;
  text: string = 'No file selected';
  canUpload: boolean = false;
  sendFileSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  getSolutionSubecription!: Subscription;
  getChallangeIdSubecription!: Subscription;
  person: Person = new Person();
  private token: string = '';
  private challangeId: number = -1;


  constructor(
    private dataStorage: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubscription = this.dataStorage.getPerson(person.username, this.token).subscribe((res) => {
        this.person = res;
        this.getChallangeIdSubecription = this.utils.challengeIdForSolutionsToEdit.subscribe((id) => {
          this.challangeId=id;
          console.log(id);
          this.getSolutionSubecription = this.dataStorage.getSolutionsForSpecificChallenge(this.token, id).subscribe((res) => {
            console.log(res);
            if(res.length>0)
              this.text = "You have an attempt sent!";
          });
        });
      });
    }
  }
  ngOnDestroy(): void {
    if (this.sendFileSubscription != null) this.sendFileSubscription.unsubscribe();
    if (this.getSolutionSubecription != null) this.getSolutionSubecription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.getChallangeIdSubecription != null) this.getChallangeIdSubecription.unsubscribe();
  }

  onUpload() {
    if (this.selectedFile) {
      this.sendFileSubscription = this.dataStorage.addSolution(this.token, this.person.personId, this.challangeId, this.selectedFile).subscribe((res) => {
        this.toastr.success("Solution uploaded succesfully");
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