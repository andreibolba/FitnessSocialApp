import { Person } from "./person.model";

export class Task{
    public taskId:number;
    public taskName:string;
    public taskDescription:string;
    public trainerId:number;
    public trainer:Person;
    public dataOfPost:Date;

    constructor(){
        this.taskId=-1;
        this.taskName='';
        this.taskDescription='';
        this.trainerId=-1;
        this.trainer=new Person();
        this.dataOfPost=new Date();
    }
}