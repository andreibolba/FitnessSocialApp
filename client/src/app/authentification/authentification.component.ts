import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-authentification',
  templateUrl: './authentification.component.html',
  styleUrls: ['./authentification.component.css'],
})
export class AuthentificationComponent implements OnInit, OnDestroy {
  isLoginMode = true;

  constructor() {}

  ngOnInit(): void {
    this.isLoginMode = true;
  }
  ngOnDestroy(): void {
    this.isLoginMode = true;
  }

  onSwitchMode(form: NgForm) {
    this.isLoginMode = !this.isLoginMode;
    form.reset();
  }

  onSubmit(form: NgForm) {
    form.reset();
  }
}
