import { Person } from './person.model';

export class Group {
  public groupId: number;
  public trainerId: number;
  public groupName: string;
  public description: string;
  public trainer: Person;
  public allInterns: Person[];
  public picture:ArrayBuffer | null;
  constructor() {
    this.groupId = -1;
    this.trainerId = -1;
    this.groupName = '';
    this.description = '';
    this.trainer = new Person();
    this.allInterns=[];
    this.picture=null;
  }
}
