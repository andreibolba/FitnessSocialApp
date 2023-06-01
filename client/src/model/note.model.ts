import { Person } from "./person.model";

export class Note{
    public noteId:number;
    public noteTitle:string;
    public noteBody:string;
    public personId:number;
    public person:Person;
    public postingDate:Date;

    constructor(){
        this.noteId=-1;
        this.noteTitle='';
        this.noteBody='';
        this.personId=-1;
        this.person=new Person();
        this.postingDate=new Date();
    }
}