export class Person{
  public id: number;
  public firstName:string;
  public lastName:string;
  public email:string;
  public username:string;
  public status:string;
  public birthDate:Date;
  constructor(){
    this.id=-1;
    this.firstName='';
    this.lastName='';
    this.email='';
    this.username='';
    this.status='';
    this.birthDate=new Date();
  }
}
