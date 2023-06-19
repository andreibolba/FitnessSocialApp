import { Person } from './person.model';
import { Picture } from './picture.model';

export class Group {
  public groupId: number;
  public trainerId: number;
  public groupName: string;
  public description: string;
  public trainer: Person;
  public allInterns: Person[];
  public pictureId: number;
  public picture: Picture;
  constructor() {
    this.groupId = -1;
    this.trainerId = -1;
    this.groupName = '';
    this.description = '';
    this.trainer = new Person();
    this.allInterns=[];
    this.pictureId=-1;
    this.picture=new Picture();
  }
}
