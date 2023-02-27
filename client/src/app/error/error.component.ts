import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit,OnDestroy{
  errorNumber:number=-1;
  errorTitle:string='';
  errorMessage:string='';

  private utilsSub!:Subscription;

  constructor(private utils:UtilsService){}

  ngOnDestroy(): void {
    if(this.utilsSub!=null)this.utilsSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utilsSub=this.utils.error.subscribe((res)=>{
      if(res==null){
        this.errorNumber=404;
        this.errorTitle='OPPS! PAGE NOT FOUND';
        this.errorMessage='Sorry, the page you\'re looking for doesn\'t exist.';
      }else{
        this.errorNumber=res.errorCode;
        this.errorTitle=this.errorTitle;
        this.errorMessage=res.errorMessage;
      }
    });
  }

}
