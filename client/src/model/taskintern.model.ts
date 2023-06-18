import { Person } from "./person.model";

export class TaskIntern{
    public internId:number;
    public intern:Person;
    public isChecked:boolean;

    constructor(){
        this.intern=new Person();
        this.internId=-1;
        this.isChecked=false;
    }
}