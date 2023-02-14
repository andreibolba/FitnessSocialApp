import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  dashboardChanged=new EventEmitter<boolean>(true);

  constructor() { }
}
