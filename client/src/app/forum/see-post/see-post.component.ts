import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-see-post',
  templateUrl: './see-post.component.html',
  styleUrls: ['./see-post.component.css']
})
export class SeePostComponent implements OnInit, OnDestroy{
  date: Date=new Date();

  constructor(){

  }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }


}
