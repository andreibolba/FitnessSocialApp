import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-my-group',
  templateUrl: './my.group.component.html',
  styleUrls: ['./my.group.component.css']
})
export class MyGroupComponent implements OnInit{

  constructor(private utils:UtilsService){

  }

  ngOnInit(): void {
    this.utils.initializeError();
  }

}
