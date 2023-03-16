import { Time } from "@angular/common";

export class Meeting {
  meetindId:number;
  meetingName:string;
  meetingLink:string;
  startTime!:Time;
  finishTime!:Time;
  traierId:number;
  internId:number;
  groupId:number;

  constructor(){
    this.traierId=-1;
    this.meetindId=-1;
    this.internId=-1;
    this.groupId=-1;
    this.meetingName='';
    this.meetingLink='';
  }
}
