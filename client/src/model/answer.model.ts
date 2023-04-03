import { Question } from "./question.model";

export class Answer{
  public questionId:number;
  public quesntion:Question;
  public internOption:string;

  constructor(questionId:number,question:Question,internOption:string){
    this.questionId=questionId;
    this.quesntion=question;
    this.internOption=internOption;
  }
}
