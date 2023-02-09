import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-authentification',
  templateUrl: './authentification.component.html',
  styleUrls: ['./authentification.component.css']
})
export class AuthentificationComponent implements OnInit, OnDestroy{
  constructor(private auth:AuthService){}
  ngOnDestroy(): void {
    this.auth.displayChanged.emit(true);
  }

  ngOnInit(): void {
    this.auth.displayChanged.emit(false);
  }

}

