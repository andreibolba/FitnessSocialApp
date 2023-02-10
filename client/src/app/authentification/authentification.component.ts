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
    const email = form.value.email;
    const password = form.value.password;
    console.log(email);
    console.log(password);
    form.reset();
  }

  private verifyPassword(password: string): string {
    let formRes = '';
    if (/[A-Z]/.test(password) == false)
      formRes = formRes.concat(
        "Password doesn't have capital letters! It must have at least 1 capital letter!\n"
      );
    if (/[a-z]/.test(password) == false)
      formRes = formRes.concat(
        "Password doesn't have small letters! It must have at least 1 small letter!\n"
      );
    if (/[0-9]/.test(password) == false)
      formRes = formRes.concat(
        "Password doesn't have digits! It must have at least 1 digit!\n"
      );
    if (/(?=.*\W)/.test(password) == false)
      formRes = formRes.concat(
        "Password doesn't have any special charaters! It must have at least 1 special charater!\n"
      );
    return formRes;
  }

  private calculateAge(birthMonth:number, birthDay:number, birthYear:number) {
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    var currentMonth = currentDate.getMonth();
    var currentDay = currentDate.getDate();
    var calculatedAge = currentYear - birthYear;

    if (currentMonth < birthMonth - 1) {
        calculatedAge--;
    }
    if (birthMonth - 1 == currentMonth && currentDay < birthDay) {
        calculatedAge--;
    }
    return calculatedAge;
}

  onSignUpSubmit(form: NgForm) {
    const email = form.value.email;
    const password = form.value.password;
    const username = form.value.username;
    const fName = form.value.fName;
    const lName = form.value.lName;
    const birthdate = form.value.birthDate;
    var dateParts = birthdate.split("-");
    var age = this.calculateAge(+dateParts[2], +dateParts[0], +dateParts[0]);

    const res = this.verifyPassword(password);
    if (res == '') {
      if(age<13){
        console.log("Trebuie sa ai cel putin 13 ani");
      }else{
      form.reset();
      }
    } else {
      console.log(res);
    }
  }
}
