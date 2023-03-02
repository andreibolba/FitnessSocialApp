export class InternGroup{
  public internId:number;
  public firstName:string;
  public lastName:string;
  public username:string;
  public isChecked:boolean;

  constructor(){
    this.internId=-1;
    this.firstName='';
    this.lastName='';
    this.username='';
    this.isChecked=false;
  }
}
