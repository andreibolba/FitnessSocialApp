import { Person } from './person.model';

export class Question {
  public questionId: number;
  public trainerId: number;
  public trainer: Person;
  public questionName: string;
  public a!: string | null;
  public b!: string | null;
  public c!: string | null;
  public d!: string | null;
  public e!: string | null;
  public f!: string | null;
  public correctOption: string;
  public points: number;
  public canBeEdited: boolean;
  constructor() {
    this.questionId = -1;
    this.trainerId = -1;
    this.trainer = new Person();
    this.questionName = '';
    this.a = null;
    this.b = null;
    this.c = null;
    this.d = null;
    this.e = null;
    this.f = null;
    this.correctOption = '';
    this.points = -1;
    this.canBeEdited = false;
  }
}
