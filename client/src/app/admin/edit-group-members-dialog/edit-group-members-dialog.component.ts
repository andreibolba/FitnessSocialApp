import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

export class CheckBox {
  id:number;
  name: string;
  value: string;
  checked:boolean;

  constructor(id:number,name: string, value: string, checked:boolean) {
    this.name = name;
    this.id=id;
    this.value = value;
    this.checked=checked;
  }
}

@Component({
  selector: 'app-edit-group-members-dialog',
  templateUrl: './edit-group-members-dialog.component.html',
  styleUrls: ['./edit-group-members-dialog.component.css'],
})
export class EditGroupMembersDialogComponent {
  dataPeopleSub!: Subscription;
  groupSub!: Subscription;
  utilsSub!: Subscription;

  group!: Group | null;
  options: CheckBox[] = [];

  private token: string = '';
  private currentId = -1;

  constructor(
    private dataService: DataStorageService,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditGroupMembersDialogComponent>,
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    this.groupSub = this.utils.groupToEdit.subscribe((res) => {
      this.group = res;
    });
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;

      let group:any;

      this.dataPeopleSub = this.dataService
        .getAllInternsInGroup(this.token, this.group!.groupId)
        .subscribe((res) => {
          res.forEach((element) => {
            this.options.unshift(
              new CheckBox(element.internId,element.firstName + " " + element.lastName, element.username,element.isChecked)
            );
          });
        });
    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
    if (this.groupSub) this.groupSub.unsubscribe();
  }

  selectOrganization(id: string) {
    this.currentId = +id;
  }

  valueChange(op: CheckBox){
    op.checked=!op.checked;
  }

  save(){
    var selected=this.options.filter(op=>op.checked==true);
    var ids:string='';
    selected.forEach(element => {
      ids+=element.id.toString()+"_";
    });
    this.dataService.updateInternInGroup(this.token,ids,this.group!.groupId).subscribe(()=>{
      this.toastr.success("Dates are update succesfully!");
      this.dialogRef.close();
    },(error)=>{
      this.toastr.error(error.error);
      console.log(error.error);
    })
  }
}
