import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

export class TrainerCombo {
  public TrainerId: number;
  public TrainerName: string;

  constructor() {
    this.TrainerId = -1;
    this.TrainerName = '';
  }
}

@Component({
  selector: 'app-edit-group-dialog',
  templateUrl: './edit-group-dialog.component.html',
  styleUrls: ['./edit-group-dialog.component.css'],
})
export class EditGroupDialogComponent implements OnInit, OnDestroy {
  @Input() groupData = {
    groupName: '',
    groupDescription:'',
    trainer: new TrainerCombo(),
  };

  dataPeopleSub!: Subscription;
  groupSub!: Subscription;
  utilsSub!: Subscription;
  opration: string = '';
  show:boolean=false;

  trainers: TrainerCombo[] = [];
  group!: Group | null;

  private token: string = '';
  private currentId=-1;

  constructor(
    private dataService: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditGroupDialogComponent>
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    this.opration = 'Add';
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.dataPeopleSub = this.dataService
        .getPeople(person.token)
        .subscribe((data) => {
          let obj = data.find((t)=>t.username ==person.username);
          this.show= obj?.status=='Admin';
          data = data.filter((t) => t.status == 'Trainer');
          data.forEach((element) => {
            let trainer: TrainerCombo = {
              TrainerId: element.personId,
              TrainerName: element.firstName + ' ' + element.lastName,
            };
            this.trainers.unshift(trainer);
          });
        },(error)=>{
          console.log(error.error);
        },()=>{
          this.groupSub = this.utils.groupToEdit.subscribe((res) => {
            this.group = res;
          });

          if (this.group == null) {
            this.opration = 'Add';
            this.groupData.trainer = this.trainers[0];
            this.currentId=this.trainers[0].TrainerId;
          } else {
            this.opration = 'Edit';
            this.groupData.groupName = this.group.groupName;
            this.groupData.groupDescription = this.group.description;
            this.currentId=this.group?.trainer.personId;

            let index=this.trainers.findIndex(g=>g.TrainerId==this.group?.trainer.personId);
            if(index!=-1){
              [this.trainers[0],this.trainers[index]]=[this.trainers[index],this.trainers[0]];
            }
          }
        });

    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
    if (this.groupSub) this.groupSub.unsubscribe();
  }

  selectOrganization(id:string){
    this.currentId=+id;
  }

  onSignUpSubmit(form: NgForm) {
    let group = new Group();
    group.groupName = form.value.groupName;
    group.description = form.value.groupDescription;
    group.trainerId = this.currentId;

    if (this.opration == 'Add') {
      this.dataService.addGroup(this.token,group).subscribe(
        () => {
          this.toastr.success('A group was added succesfully!');
          this.dialogRef.close();;
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    } else {
      group.groupId = this.group!.groupId;
      console.log(group);
      this.dataService.editGroup(this.token,group).subscribe(
        () => {
          this.toastr.success('The edit was succesfully!');
          this.dialogRef.close();
        },
        (error) => {
          console.log(error.error);
          this.toastr.error(error.error);
        }
      );
    }
  }
}
