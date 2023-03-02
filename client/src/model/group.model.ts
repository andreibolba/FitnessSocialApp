import { Person } from './person.model';

export class Group {
  public groupId: number;
  public trainerId: number;
  public groupName: string;
  public trainer: Person;
  public allInterns: Person[];
  constructor() {
    this.groupId = -1;
    this.trainerId = -1;
    this.groupName = '';
    this.trainer = new Person();
    this.allInterns=[];
  }
}
