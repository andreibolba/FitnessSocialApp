import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-list-of-people-chat',
  templateUrl: './list-of-people-chat.component.html',
  styleUrls: ['./list-of-people-chat.component.css']
})
export class ListOfPeopleChatComponent implements OnDestroy{
  selectChatSubscription!:Subscription;

  constructor(private utils:UtilsService){

  }

  ngOnDestroy(): void {
    if(this.selectChatSubscription!=null) this.selectChatSubscription.unsubscribe();
  }

  onChatClick(){
    this.utils.selectChat.next(true);
  }
}
