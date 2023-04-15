export class Person{
  public personId: number;
  public firstName:string;
  public lastName:string;
  public email:string;
  public username:string;
  public status:string;
  public birthDate:Date;
  public karma:number;
  public answers: number;
  constructor(){
    this.personId=-1;
    this.firstName='';
    this.lastName='';
    this.email='';
    this.username='';
    this.status='';
    this.birthDate=new Date();
    this.karma=-1;
    this.answers=-1;
  }
}
