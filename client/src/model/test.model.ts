import { Person } from "./person.model";
import { Question } from "./question.model";

export class Test{
  public testId:number;
  public testName: string;
  public questions:Question[];
  public points:number;
  public trainerId: number;
  public trainer:Person;
  public dateOfPost: Date;
  public deadline:Date;
  public canBeEdited: boolean;

  constructor(){
    this.testId=-1;
    this.testName='';
    this.questions=[];
    this.points=0;
    this.trainerId=-1;
    this.trainer=new Person();
    this.dateOfPost=new Date();
    this.deadline=new Date();
    this.canBeEdited=false;
  }
}
