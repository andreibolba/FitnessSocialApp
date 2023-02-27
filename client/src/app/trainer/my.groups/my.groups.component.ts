import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-my-groups',
  templateUrl: './my.groups.component.html',
  styleUrls: ['./my.groups.component.css']
})
export class MyGroupsComponent implements OnInit{

  constructor(private utils:UtilsService){

  }

  ngOnInit(): void {
    this.utils.initializeError();
  }

}
