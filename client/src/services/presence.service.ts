import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { Person } from 'src/model/person.model';
import { LogService } from './log.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService{
  hubUrl='https://localhost:7191/hubs/';
  private hubConnection? :HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();


  constructor(private toastr:ToastrService,private log:LogService) { }

  createHubConnection(user:Person, token:string){
    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl+'presence',{
      accessTokenFactory:()=>token
    }).withAutomaticReconnect().build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline',username=>{
      //this.toastr.info(username + 'has connected')
    });

    this.hubConnection.on('UserIsOffline',username=>{
      //this.toastr.warning(username + 'has disconnected')
    });
    
    this.hubConnection.on('GetOnlineUsers', users=>{
      this.onlineUsersSource.next(users);
    })
    
  }

  stopHubConnection(){
    this.hubConnection?.stop().catch(error=>console.log(error));
  }
}
