import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Ranking } from 'src/model/ranking.mode';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-rankings',
  templateUrl: './rankings.component.html',
  styleUrls: ['./rankings.component.css']
})
export class RankingsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'intern',
    'points'
  ];
  hasTableValues:boolean=false;
  dataSource!: MatTableDataSource<Ranking>;
  rankings!: Ranking[];
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  getRankingsSubscription!: Subscription;

  constructor(private dataStorage: DataStorageService, private utils: UtilsService) {

  }
  ngOnDestroy(): void {
    this.utils.isOnChallanges.next(true);
    if(this.getRankingsSubscription!=null)this.getRankingsSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.utils.initializeError();
    this.utils.isOnChallanges.next(false);
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.getRankingsSubscription = this.dataStorage.challengeData.rankings(person.token).subscribe((res)=>{
        this.rankings=res;
        this.dataSource = new MatTableDataSource(this.rankings);
        this.dataSource.paginator = this.paginator;
        if(this.dataSource.data.length==0){
          this.hasTableValues=false;
        }else{
          this.hasTableValues = true;
        }
      });
    }
  }

  

}
