import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/services/auth.service';
import { DataStorageService } from 'src/services/data-storage.service';
import { LogService } from 'src/services/log.service';

@Component({
  selector: 'app-authentification',
  templateUrl: './authentification.component.html',
  styleUrls: ['./authentification.component.css'],
})
export class AuthentificationComponent implements OnInit {
  isLoginMode = false;
  isForgotPassword = false;
  isRecoverPassword = false;
  error: string | any = '';
  linkId: string | null = '';
  sub!: Subscription;

  constructor(
    private authService: AuthService,
    private dataService: DataStorageService,
    private router: Router,
    private log: LogService,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.authService.logout();
    if (this.router.url == '/') {
      this.isLoginMode = true;
      this.isForgotPassword = false;
      this.isRecoverPassword = false;
    } else {
      this.isLoginMode = false;
      this.isForgotPassword = false;
      this.isRecoverPassword = true;
      this.sub = this.activatedRoute.paramMap.subscribe((params) => {
        this.linkId = params.get('linkid');
        if(this.linkId)
          this.dataService.isLinkValid(+this.linkId).subscribe( () => {},
          (error) => {
            this.toastr.error(error.error);
            this.router.navigate(['']);
          });
      });

    }
    // this.log
    //   .log({
    //     LogType: 'Info',
    //     LogMessage: 'A user wants to log in!',
    //     DateOfLog: new Date(),
    //   })
    //   .subscribe(
    //     (resData) => {
    //       console.log('ok');
    //     },
    //     (error) => {
    //       this.error = error;
    //       console.log(error.error);
    //     }
    //   );
  }

  onLogInSubmit(form: NgForm) {
    const email = form.value.email;
    const password = form.value.password;
    let model = { email, password };

    this.authService.login(model).subscribe(
      (resData) => {
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        this.error = error;
        this.toastr.error(error.error);
      }
    );
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

  onSignUpSubmit(form: NgForm) {
    const email = form.value.email;
    const password = form.value.password;
    const username = form.value.username;
    const fName = form.value.fName;
    const lName = form.value.lName;
    const birthdate = form.value.birthDate;

    const res = this.verifyPassword(password);
    if (res == '') {
      form.reset();
    } else {
      console.log(res);
    }
  }

  onRecover(form: NgForm) {}

  onForgot(form: NgForm) {
    let email = form.value.email;
    this.dataService.sendEmail(email).subscribe(
      () => {
        this.toastr.success('Mail was sent succesfully!');
      },
      (error) => {
        this.error = error;
        this.toastr.error(error.error);
      }
    );
  }

  onRecoverPassword() {
    this.isLoginMode = false;
    this.isForgotPassword = true;
    this.isRecoverPassword = false;
  }

  onRememberPassword() {
    this.isLoginMode = true;
    this.isForgotPassword = false;
    this.isRecoverPassword = false;
  }

  onGoBack() {
    this.router.navigate(['']);
  }
}
