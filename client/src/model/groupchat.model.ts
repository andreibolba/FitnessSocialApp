import { Person } from "./person.model";
import { Picture } from "./picture.model";

export class GroupChat{
  public groupChatId:number;
  public groupChatName:string;
  public groupChatDescription:string | null;
  public adminId:number;
  public admin:Person;
  public participants:Person[];
  public pictureId:number;
  public picture:Picture;

  constructor(){
    this.groupChatId = -1;
    this.groupChatName = '';
    this.groupChatDescription = null;
    this.adminId = -1;
    this.admin = new Person();
    this.participants = [];
    this.pictureId=-1;
    this.picture=new Picture();
  }
}
