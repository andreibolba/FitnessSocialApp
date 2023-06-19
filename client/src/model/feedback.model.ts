import { Challenge } from "./challenge.model";
import { Person } from "./person.model";
import { Task } from "./task.model";
import { Test } from "./test.model";

export class Feedback{
    public feedbackId:number;
    public trainerId:number;
    public internId:number;
    public taskId:number | null;
    public challangeId:number | null;
    public testId:number | null;
    public content:string;
    public dateOfPost:Date;
    public challange:Challenge | null;
    public intern:Person;
    public task:Task | null;
    public test:Test | null;
    public trainer:Person;
    public grade:number;

    constructor(){
        this.feedbackId=-1;
        this.trainerId=-1;
        this.internId=-1;
        this.taskId=-1;
        this.challangeId=-1;
        this.testId=-1;
        this.content='';
        this.dateOfPost=new Date();
        this.challange = null;
        this.task = null;
        this.intern = new Person();
        this.test = null;
        this.trainer = new Person();
        this.grade=-1;
    }

}