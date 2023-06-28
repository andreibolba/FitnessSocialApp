import { Picture } from "./picture.model";

export class Person{
  public personId: number;
  public firstName:string;
  public lastName:string;
  public email:string;
  public username:string;
  public status:string;
  public birthDate:Date;
  public karma:number;
  public postsNumber: number;
  public commentsNumber: number;
  public pictureId: number;
  public picture: Picture;
  constructor(){
    this.personId=-1;
    this.firstName='';
    this.lastName='';
    this.email='';
    this.username='';
    this.status='';
    this.birthDate=new Date();
    this.karma=-1;
    this.postsNumber=-1;
    this.commentsNumber=-1;
    this.pictureId=-1;
    this.picture=new Picture();
  }
}
