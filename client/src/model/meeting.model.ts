import { Person } from "./person.model";

export class Meeting {
  public meetingId:number;
  public meetingName:string;
  public meetingLink:string;
  public meetingStartTime:Date;
  public meetingFinishTime:Date;
  public traierId:number;
  public internId:number;
  public groupId:number;
  public internIds:string;
  public groupIds:string;
  public allPeopleInMeeting:Person[];
  public participants:string;

  constructor(){
    this.traierId=-1;
    this.meetingId=-1;
    this.internId=-1;
    this.groupId=-1;
    this.meetingName='';
    this.meetingLink='';
    this.internIds='';
    this.groupIds='';
    this.participants='';
    this.meetingStartTime=new Date();
    this.meetingFinishTime=new Date();
    this.allPeopleInMeeting=[];
  }
}
