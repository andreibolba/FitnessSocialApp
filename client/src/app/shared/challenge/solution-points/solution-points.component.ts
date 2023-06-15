import { Component, OnDestroy, OnInit } from '@angular/core';
import { Options } from "ng5-slider";
import { Subscription } from 'rxjs';
import { UtilsService } from 'src/services/utils.service';
import { Ng5SliderModule } from 'ng5-slider';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-solution-points',
  templateUrl: './solution-points.component.html',
  styleUrls: ['./solution-points.component.css']
})
export class SolutionPointsComponent implements OnInit, OnDestroy{
  getPointsSubscription!:Subscription;
  getMaxPointsSubscription!:Subscription;
  value: number = 0;
  maxValue:number = 0;

  constructor(private utils:UtilsService, private dialog:MatDialogRef<SolutionPointsComponent>){

  }


  ngOnInit(): void {
    this.getPointsSubscription = this.utils.pointToSolutionApprove.subscribe((res)=>{
      if(res)
      {
        this.value = res;
      }
      this.getMaxPointsSubscription = this.utils.maxpointToSolutionApprove.subscribe((data)=>{
        this.maxValue = data;
      });
    });
  }

  ngOnDestroy(): void {
    this.utils.pointToSolutionApprove.next(null);
    if(this.getPointsSubscription!=null) this.getPointsSubscription.unsubscribe();
    if(this.getMaxPointsSubscription!=null) this.getMaxPointsSubscription.unsubscribe();
  }

  onClose(){
    this.utils.pointToSolutionApprove.next(this.value);
    this.dialog.close();
  }

}
