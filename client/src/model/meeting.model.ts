import { Time } from "@angular/common";

export class Meeting {
  public meetindId:number;
  public meetingName:string;
  public meetingLink:string;
  public startTime:Date;
  public finishTime:Date;
  public traierId:number;
  public internId:number;
  public groupId:number;

  constructor(){
    this.traierId=-1;
    this.meetindId=-1;
    this.internId=-1;
    this.groupId=-1;
    this.meetingName='';
    this.meetingLink='';
    this.startTime=new Date();
    this.finishTime=new Date()
  }
}
