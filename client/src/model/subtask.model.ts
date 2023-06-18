import { Task } from "./task.model";

export class SubTask{
    public subTaskId:number;
    public subTaskName:string;
    public taskId:number;
    public task:Task;

    constructor(){
        this.subTaskId=-1;
        this.taskId=-1;
        this.subTaskName='';
        this.task=new Task();
    }
}