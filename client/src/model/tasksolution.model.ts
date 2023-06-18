import { Person } from "./person.model";
import { Task } from "./task.model";

export class TaskSolution{
    public taskSolutionId:number;
    public internId:number;
    public taskId:number;
    public task:Task;
    public intern:Person;
    public dateOfSolution:Date;
    public solutionFile:File | null;

    constructor(){
        this.taskSolutionId=-1;
        this.taskId=-1;
        this.internId=-1;
        this.dateOfSolution=new Date();
        this.task=new Task();
        this.intern=new Person();
        this.solutionFile = null;
    }
}