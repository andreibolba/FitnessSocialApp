export class Meeting {
  public meetingId:number;
  public meetingName:string;
  public meetingLink:string;
  public meetingStartTime:Date;
  public meetingFinishTime:Date;
  public traierId:number;
  public internId:number;
  public groupId:number;

  constructor(){
    this.traierId=-1;
    this.meetingId=-1;
    this.internId=-1;
    this.groupId=-1;
    this.meetingName='';
    this.meetingLink='';
    this.meetingStartTime=new Date();
    this.meetingFinishTime=new Date()
  }
}
