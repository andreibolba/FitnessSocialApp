import { Group } from "./group.model";

export class TaskGroup{
    public groupId:number;
    public group:Group;
    public isChecked:boolean;

    constructor(){
        this.group=new Group();
        this.groupId=-1;
        this.isChecked=false;
    }
}