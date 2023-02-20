import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-create-edit-dialog',
  templateUrl: './create-edit-dialog.component.html',
  styleUrls: ['./create-edit-dialog.component.css']
})
export class CreateEditDialogComponent implements OnInit, OnDestroy {
  dataPeopleSub!: Subscription;
  status: string = '';
  opration: string = '';

  constructor(
    private dataService: DataStorageService,
    private router: Router
  ) {}

  ngOnInit(): void {
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
    this.opration='Add';
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
  }

  onSignUpSubmit(form:NgForm){
    const email = form.value.email;
    const username = form.value.username;
    const fName = form.value.fName;
    const lName = form.value.lName;
    const birthdate = form.value.birthDate;

    console.log(email);
  }
}
