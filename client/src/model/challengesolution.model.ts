import { Challenge } from "./challenge.model";
import { Person } from "./person.model";

export class ChallengeSolution {
    public challangeSolutionId:number;
    public challangeId:number;
    public challange:Challenge;
    public dateOfSolution:Date;
    public internId:number;
    public intern:Person;
    public solutionContent:string;
    public solutionFile:File | null;
    public approved:boolean;

    constructor(){
        this.challangeSolutionId=-1;
        this.challangeId=-1;
        this.challange=new Challenge();
        this.internId=-1;
        this.intern=new Person();
        this.dateOfSolution=new Date();
        this.solutionContent='';
        this.approved=false;
        this.solutionFile=null;
    }
}