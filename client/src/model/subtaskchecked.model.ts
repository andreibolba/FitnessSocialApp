import { Person } from "./person.model";
import { Task } from "./task.model";

export class SubTaskChecked{
    public subTaskCheckedId:number;
    public taskId:number;
    public task:Task;
    public internId:number;
    public intern:Person;

    constructor(){
        this.subTaskCheckedId=-1;
        this.taskId=-1;
        this.internId=-1;
        this.intern=new Person();
        this.task=new Task();
    }
}