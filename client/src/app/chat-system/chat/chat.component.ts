import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy{
  haveChat=false;
  haveChatSubscription!:Subscription;

  constructor(private utils:UtilsService){

  }
  ngOnInit(): void {
    this.haveChatSubscription = this.utils.selectChat.subscribe((data)=>{
      this.haveChat = data;
    });
  }



  ngOnDestroy(): void {
    if(this.haveChatSubscription!=null)this.haveChatSubscription.unsubscribe();
  }


}
