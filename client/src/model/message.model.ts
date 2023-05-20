import { Person } from "./person.model";

export class Message{
  public id:number;
  public message:string;
  public personSenderId:number;
  public personSender:Person;
  public personReceiverId:number;
  public personReceiver:Person;
  public sendDate:Date;
  public chatPerson:Person;
  public youOrThem:string;

  constructor(){
    this.id=-1;
    this.message='';
    this.personReceiverId=-1;
    this.personReceiver=new Person();
    this.personSenderId=-1;
    this.personSender=new Person();
    this.sendDate=new Date();
    this.chatPerson=new Person();
    this.youOrThem='';
  }
}
