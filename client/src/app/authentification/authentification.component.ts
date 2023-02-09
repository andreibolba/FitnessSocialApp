import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-authentification',
  templateUrl: './authentification.component.html',
  styleUrls: ['./authentification.component.css'],
})
export class AuthentificationComponent implements OnInit, OnDestroy {
  isLoginMode = false;

  constructor() {}

  ngOnInit(): void {
    this.isLoginMode = false;
  }
  ngOnDestroy(): void {
    this.isLoginMode = false;
  }

  onSwitchMode(form: NgForm) {
    this.isLoginMode = !this.isLoginMode;
    form.reset();
  }

  onLogInSubmit(form: NgForm) {
    const email=form.value.email;
    const password=form.value.password;
    console.log(email);
    console.log(password);
    form.reset();
  }

  onSignUpSubmit(form: NgForm) {
    const email=form.value.email;
    const password=form.value.password;
    const username=form.value.username;
    const fName=form.value.fName;
    const lName=form.value.lName;
    const birthdate=form.value.birthDate;
    console.log(email);
    console.log(password);
    console.log(username);
    console.log(fName);
    console.log(lName);
    console.log(birthdate);
    form.reset();
  }
}
