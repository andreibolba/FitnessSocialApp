import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { GroupChat } from 'src/model/groupchat.model';
import { GroupChatMessage } from 'src/model/groupchatmessage.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditGroupChatComponent } from '../add-edit-group-chat/add-edit-group-chat.component';
import { UploadPhotoComponent } from 'src/app/shared/upload-photo/upload-photo.component';

@Component({
  selector: 'app-group-chat-details',
  templateUrl: './group-chat-details.component.html',
  styleUrls: ['./group-chat-details.component.css']
})
export class GroupChatDetailsComponent implements OnInit, OnDestroy {
  groupChatSubscription!: Subscription;
  getCurrentPersonSubscription!: Subscription;
  deleteGroupChatSubscription!: Subscription;
  deleteMemberGroupChatSubscription!: Subscription;
  leaveGroupChatSubscription!: Subscription;
  makeAdminGroupChatSubscription!: Subscription;
  loggedPerson: Person = new Person();
  groupChat: GroupChat = new GroupChat();
  panelOpenState = false;
  descriptionOfGroup = 'lorem';

  private token: string = '';

  constructor(private utils: UtilsService, private dataStorage: DataStorageService, private router: Router, private toastr: ToastrService, private dialog: MatDialog) {

  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getCurrentPersonSubscription = this.dataStorage
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          this.loggedPerson = data;
          this.groupChatSubscription = this.utils.groupChatPersonChat.subscribe((res) => {
            if (res)
              this.groupChat = res;
          });
        });
    }
  }
  ngOnDestroy(): void {
    if (this.getCurrentPersonSubscription != null) this.getCurrentPersonSubscription.unsubscribe();
    if (this.groupChatSubscription != null) this.groupChatSubscription.unsubscribe();
    if (this.deleteGroupChatSubscription != null) this.deleteGroupChatSubscription.unsubscribe();
    if (this.deleteMemberGroupChatSubscription != null) this.deleteMemberGroupChatSubscription.unsubscribe();
    if (this.leaveGroupChatSubscription != null) this.leaveGroupChatSubscription.unsubscribe();
  }

  onBack() {
    this.utils.selectChat.next(2);
  }

  onDelete() {
    this.deleteGroupChatSubscription = this.dataStorage.deleteGroupChat(this.token, this.groupChat.groupChatId).subscribe(() => {
      this.toastr.success("GroupChat deleted succesfully!");
      let res = new GroupChatMessage();
      res.groupChatId = this.groupChat.groupChatId;
      this.utils.newGroupChatMessage.next(res);
      this.utils.selectChat.next(-1);
    });
  }

  onRemove(person: Person) {
    this.deleteMemberGroupChatSubscription = this.dataStorage.removeMemberFromGroupChat(this.token, this.groupChat.groupChatId, person.personId).subscribe(() => {
      this.toastr.success("Member removed succesfully!");
      let index = this.groupChat.participants.findIndex((r) => r.personId == person.personId);
      this.groupChat.participants.splice(index, 1);
    });
  }

  onMakeAdmin(person: Person) {
    this.makeAdminGroupChatSubscription = this.dataStorage.makeAdminGroupChat(this.token, this.groupChat.groupChatId, person.personId).subscribe(() => {
      this.toastr.success("Made admin succesfully!");
      this.groupChat.adminId = person.personId;
    });
  }

  onLeave() {
    this.leaveGroupChatSubscription = this.dataStorage.removeMemberFromGroupChat(this.token, this.groupChat.groupChatId, this.loggedPerson.personId).subscribe(() => {
      this.toastr.success("Group leaved succesfully!");
      let index = this.groupChat.participants.findIndex((r) => r.personId == this.loggedPerson.personId);
      this.groupChat.participants.splice(index, 1);
      let res = new GroupChatMessage();
      res.groupChatId = this.groupChat.groupChatId;
      this.utils.newGroupChatMessage.next(res);
      this.utils.selectChat.next(-1);
    });
  }

  openDialog(op:number) {
    const dialogRef = op==1?  this.dialog.open(AddEditGroupChatComponent): this.dialog.open(UploadPhotoComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  onAdd() {
    this.utils.editGroupChatOption.next(3);
    this.utils.groupChatPersonChat.next(this.groupChat);
    this.openDialog(1);
  }

  onEdit() {
    this.utils.editGroupChatOption.next(2);
    this.utils.groupChatPersonChat.next(this.groupChat);
    this.openDialog(1);
  }

  onEditPic(groupChat:GroupChat){
    this.utils.idToPictureUpload.next(2);
    this.utils.idOfGroupChatToPictureUpload.next(groupChat.groupChatId);
    this.openDialog(2);
  }

}
