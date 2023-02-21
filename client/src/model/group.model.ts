import { Person } from './person.model';

export class Group {
  constructor(
    public groupId: number,
    public name: string,
    public trainer: Person,
    public membersCount: number
  ) {}
}
