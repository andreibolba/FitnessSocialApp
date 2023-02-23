import { Person } from './person.model';

export class InternGroup{
  public internId:number;
  public intern:Person;
  public isChecked:boolean;

  constructor(){
    this.internId=-1;
    this.intern=new Person();
    this.isChecked=false;
  }
}
