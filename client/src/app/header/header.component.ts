import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/services/auth.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit,OnDestroy{
  sub!:Subscription;

  constructor(private authSer:AuthService) {

  }
  ngOnInit(): void {
    this.sub=this.authSer.authChanged.subscribe();
  }

  ngOnDestroy(): void {
    if(this.sub)
      this.sub.unsubscribe();
  }

  register(){
    this.authSer.authChanged.next(false);
  }

  onLogOut(){
    this.authSer.logout();
  }
}
