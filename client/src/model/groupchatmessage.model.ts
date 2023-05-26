import { GroupChat } from './groupchat.model';
import { Person } from './person.model';

export class GroupChatMessage {
  public groupChatMessageId: number;
  public personId: number;
  public groupChatId: number;
  public message: string;
  public sendDate: Date;
  public groupChat: GroupChat;
  public person: Person;

  constructor(){
    this.groupChatMessageId=-1;
    this.personId=-1;
    this.groupChatId=-1;
    this.message='';
    this.sendDate=new Date();
    this.groupChat=new GroupChat();
    this.person=new Person();
  }
}
