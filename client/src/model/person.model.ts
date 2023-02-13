export class Person{
  public firstName:string;
  public lastName:string;
  public email:string;
  public username:string;
  public status:string;
  public birthDate:Date;
  constructor(){
    this.firstName='';
    this.lastName='';
    this.email='';
    this.username='';
    this.status='';
    this.birthDate=new Date();
  }
}
