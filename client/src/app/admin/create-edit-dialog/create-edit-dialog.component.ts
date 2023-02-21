import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-create-edit-dialog',
  templateUrl: './create-edit-dialog.component.html',
  styleUrls: ['./create-edit-dialog.component.css'],
})
export class CreateEditDialogComponent implements OnInit, OnDestroy {
  @Input() regForm!: NgForm;
  @Input() personData = {
    fName: '',
    lName: '',
    email: '',
    username: '',
    bdate: new Date().toISOString()
  };

  dataPeopleSub!: Subscription;
  utilsSub!: Subscription;
  status: string = '';
  opration: string = '';
  person!: Person | null;
  private token: string = '';

  constructor(
    private dataService: DataStorageService,
    private router: Router,
    private utils: UtilsService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<CreateEditDialogComponent>
  ) {}

  ngOnInit(): void {
    this.utilsSub = this.utils.userToEdit.subscribe((res) => {
      this.person = res;
    });

    if (this.person == null) {
      this.opration = 'Add';
    } else {
      this.opration = 'Edit';
      this.personData = {
        fName: this.person.firstName,
        lName: this.person.lastName,
        email: this.person.email,
        username: this.person.username,
        bdate:  new Date(this.person.birthDate).toISOString(),
      };
    }

    switch (this.router.url) {
      case '/dashboard/administrators':
        this.status = 'Admin';
        break;
      case '/dashboard/interns':
        this.status = 'Intern';
        break;
      case '/dashboard/trainers':
        this.status = 'Trainer';
        break;
      default:
        break;
    }
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
    }
  }

  ngOnDestroy(): void {
    this.utils.userToEdit.next(null);
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
  }

  onSignUpSubmit(form: NgForm) {
    let person = new Person();
    person.firstName = form.value.fName;
    person.lastName = form.value.lName;
    person.email = form.value.email;
    person.username = form.value.username;
    person.birthDate = form.value.birthDate;
    person.status = this.status;

    if (this.opration == 'Add') {
      this.dataService.addperson(person, this.token).subscribe(
        () => {
          this.toastr.success('An ' + status + ' was added succesfully!');
          this.dialogRef.close();
        },
        (error) => {
          this.toastr.error(error.error);
        }
      );
    } else {
      person.personId=this.person!.personId;
      console.log('Edit');
      console.log(person);
    }
  }
}
