import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-challange',
  templateUrl: './challange.component.html',
  styleUrls: ['./challange.component.css']
})
export class ChallangeComponent implements OnInit{

  constructor(private utils:UtilsService){

  }

  ngOnInit(): void {
    this.utils.initializeError();
  }

}
