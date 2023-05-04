import { Person } from "./person.model";

export class Comment{
  public commentId:number;
  public personId:number;
  public postId:number;
  public commentContent:string;
  public dateOfComment:Date;
  public person:Person;
  public karma:number;
  public up:string;
  public upvote:boolean;
  public down:string;
  public downvote:boolean;
  public canEdit:boolean;

  constructor(){
    this.commentId=-1;
    this.personId=-1;
    this.postId=-1;
    this.commentContent='';
    this.dateOfComment=new Date();
    this.person=new Person();
    this.karma=-1;
    this.up='';
    this.upvote=false;
    this.down='';
    this.downvote=false;
    this.canEdit=false;
  }
}
