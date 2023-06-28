import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { GroupChat } from 'src/model/groupchat.model';
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
  group: GroupChat = new GroupChat();
  nameOfGroup: string = '';
  descriptionOfGroup: string = '';
  step: number = 1;
  getAllPersonSubcription!: Subscription;
  addGroupchatSubcription!: Subscription;
  getPersonSubcription!: Subscription;
  optionSubcription!: Subscription;
  editGroupSubcription!: Subscription;
  saveEditGroupSubcription!: Subscription;
  updateMembersGroupSubcription!: Subscription;
  options: CheckBox[] = [];
  searchText: string = '';
  option: number = 1;
  private token: string = '';
  private adminId: number = -1;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private toastr: ToastrService,
    private dialog: MatDialogRef<AddEditGroupChatComponent>
  ) { }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getPersonSubcription = this.dataStorage.personData
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.adminId = data.personId;
          this.getAllPersonSubcription = this.dataStorage.personData
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
                this.optionSubcription = this.utils.editGroupChatOption.subscribe((res) => {
                  this.option = res;
                  this.step = res == 1 || res == 2 ? 1 : 2;
                  this.editGroupSubcription = this.utils.groupChatPersonChat.subscribe((data) => {
                    if (data) {
                      this.nameOfGroup = data.groupChatName;
                      this.descriptionOfGroup = data.groupChatDescription ? data.groupChatDescription : '';
                      this.group = data;
                      this.group.participants.forEach(element => {
                        let index = this.options.findIndex(c => c.id == element.personId);
                        if (index != -1)
                          this.options[index].checked = true;
                      });
                    }
                  })
                });
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
    if (this.option == 1) {
      let ids: number[] = [];
      this.options.forEach((element) => {
        if (element.checked) ids.push(element.id);
      });
      ids.push(this.adminId);
      this.addGroupchatSubcription = this.dataStorage.groupChatData
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
  }

  onNext() {
    this.step++;
  }

  valueChange(op: CheckBox) {
    op.checked = !op.checked;
  }

  onSave() {
    if (this.option == 2) {
      this.group.groupChatName = this.nameOfGroup;
      this.group.groupChatDescription = this.descriptionOfGroup == '' ? null : this.descriptionOfGroup;
      this.saveEditGroupSubcription = this.dataStorage.groupChatData.editGroupChat(this.token, this.group).subscribe(() => {
        this.toastr.success("Group updates successfully!");
        this.dialog.close();
      }, (error) => {
        this.toastr.error(error.error);
      });
    } else if (this.option == 3) {
      var ids: string = '';
      this.options.forEach(element => {
        if(element.checked==true)
        ids += element.id.toString() + "_";
      });
      ids+= this.adminId+"_";
      this.updateMembersGroupSubcription = this.dataStorage.groupChatData.updateMembersGroupChat(this.token, this.group.groupChatId, ids).subscribe((res) => {
        this.utils.groupChatPersonChat.next(res);
        this.toastr.success("Group members updates successfully!");
        this.dialog.close();
      }, (error) => {
        this.toastr.error(error.error);
      });
    }
  }
}
