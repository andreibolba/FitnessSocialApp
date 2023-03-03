export class InternGroup{
  public personId:number;
  public firstName:string;
  public lastName:string;
  public username:string;
  public isChecked:boolean;

  constructor(){
    this.personId=-1;
    this.firstName='';
    this.lastName='';
    this.username='';
    this.isChecked=false;
  }
}
