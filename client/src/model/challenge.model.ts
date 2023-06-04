import { Person } from "./person.model";

export class Challenge {
    public challangeId:number;
    public challangeName:string;
    public challangeDescription:string;
    public trainerId:number;
    public trainer:Person;
    public dateOfPost:Date;
    public deadline:Date;
    public canDelete:boolean;
    public points:number;

    constructor(){
        this.challangeId=-1;
        this.challangeName='';
        this.challangeDescription='';
        this.trainerId=-1;
        this.trainer= new Person();
        this.dateOfPost=new Date();
        this.deadline=new Date();
        this.canDelete=false;
        this.points=0;
    }
}