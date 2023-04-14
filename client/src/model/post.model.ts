import { Person } from "./person.model";

export class Post{
  public postId:number;
  public person:Person;
  public title:string;
  public content:string;
  public dateOfPost:Date;
  public karma:number;
  public views:number;

  constructor() {
    this.postId=-1;
    this.person=new Person();
    this.title='';
    this.content='';
    this.dateOfPost=new Date();
    this.karma=-1;
    this.views=-1;
  }
}
