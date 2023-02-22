import { Injectable } from "@angular/core";
import { LoggedPerson } from "src/model/loggedperson.model";
import { AuthService } from "src/services/auth.service";
@Injectable({
  providedIn: 'root',
})
export class Utils{

  constructor(private authSer:AuthService){}
}
