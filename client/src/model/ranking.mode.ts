import { Person } from "./person.model";

export class Ranking{
    public position:number;
    public personId:number;
    public person:Person;
    public points:number;

    constructor(){
        this.position=-1;
        this.personId=-1;
        this.person=new Person();
        this.points=-1;
    }
}