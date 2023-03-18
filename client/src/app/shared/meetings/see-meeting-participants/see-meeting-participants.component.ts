import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Person } from 'src/model/person.model';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-see-meeting-participants',
  templateUrl: './see-meeting-participants.component.html',
  styleUrls: ['./see-meeting-participants.component.css']
})
export class SeeMeetingParticipantsComponent implements OnInit, OnDestroy{
  people:Person[]=[];

  utilsSub!:Subscription;

  constructor(private utils:UtilsService,private toastr: ToastrService){
  }

  ngOnInit(): void {
    this.utilsSub=this.utils.meetingParticipants.subscribe((res)=>{
      if(res!=null)
        this.people=res;
        console.log(this.people);
    },(error)=>{
      this.toastr.error(error.error);
    })
  }
  ngOnDestroy(): void {
    if(this.utilsSub!=null)this.utilsSub.unsubscribe();
  }

}
