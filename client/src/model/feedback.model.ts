import { Challenge } from "./challenge.model";
import { Person } from "./person.model";
import { Task } from "./task.model";
import { Test } from "./test.model";

export class Feedback{
    public feedbackId:number;
    public personReceiverId:number;
    public personSenderId:number;
    public taskId:number | null;
    public challangeId:number | null;
    public testId:number | null;
    public content:string;
    public dateOfPost:Date;
    public challange:Challenge | null;
    public task:Task | null;
    public test:Test | null;
    public personReceiver:Person;
    public personSender:Person;
    public grade:number;

    constructor(){
        this.feedbackId=-1;
        this.personReceiverId=-1;
        this.personSenderId=-1;
        this.taskId=-1;
        this.challangeId=-1;
        this.testId=-1;
        this.content='';
        this.dateOfPost=new Date();
        this.challange = null;
        this.task = null;
        this.personReceiver = new Person();
        this.test = null;
        this.personSender = new Person();
        this.grade=-1;
    }

}