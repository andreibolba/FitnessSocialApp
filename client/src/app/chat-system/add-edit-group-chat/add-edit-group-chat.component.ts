import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

export class CheckBox {
  id: number;
  name: string;
  value: string;
  checked: boolean;

  constructor(id: number, name: string, value: string, checked: boolean) {
    this.name = name;
    this.id = id;
    this.value = value;
    this.checked = checked;
  }
}

@Component({
  selector: 'app-add-edit-group-chat',
  templateUrl: './add-edit-group-chat.component.html',
  styleUrls: ['./add-edit-group-chat.component.css'],
})
export class AddEditGroupChatComponent implements OnInit, OnDestroy {
  nameOfGroup: string = '';
  descriptionOfGroup: string = '';
  step: number = 1;
  getAllPersonSubcription!: Subscription;
  addGroupchatSubcription!: Subscription;
  getPersonSubcription!: Subscription;
  options: CheckBox[] = [];
  searchText: string = '';
  private token: string = '';
  private adminId: number = -1;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private toastr: ToastrService,
    private dialog: MatDialogRef<AddEditGroupChatComponent>
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubcription = this.dataStorage
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.adminId = data.personId;
          this.getAllPersonSubcription = this.dataStorage
            .getPeople(person.token)
            .subscribe((res) => {
              res.forEach((element) => {
                if (element.personId != data.personId)
                  this.options.unshift(
                    new CheckBox(
                      element.personId,
                      element.firstName + ' ' + element.lastName,
                      element.username,
                      false
                    )
                  );
              });
            });
        });
    }
  }
  ngOnDestroy(): void {
    if (this.addGroupchatSubcription != null)
      this.addGroupchatSubcription.unsubscribe();
    if (this.getAllPersonSubcription != null)
      this.getAllPersonSubcription.unsubscribe();
  }

  onSignUpSubmit() {
    let ids: number[] = [];
    this.options.forEach((element) => {
      if (element.checked) ids.push(element.id);
    });
    this.addGroupchatSubcription = this.dataStorage
      .addGroupChat(
        this.token,
        this.nameOfGroup,
        this.descriptionOfGroup,
        this.adminId,
        ids
      )
      .subscribe((res) => {
        this.toastr.success('Group was created!');
        this.dialog.close();
        this.utils.newGroupChatMessage.next(res);
      });
  }

  onNext() {
    this.step++;
  }

  valueChange(op: CheckBox) {
    op.checked = !op.checked;
  }
}
