import {
  Component,
  AfterViewInit,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DataStorageService } from 'src/services/data-storage.service';
import { Subscription } from 'rxjs';
import { Person } from 'src/model/person.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';


@Component({
  selector: 'app-administrators',
  templateUrl: './administrators.component.html',
  styleUrls: ['./administrators.component.css'],
})
export class AdministratorsComponent implements OnInit,OnDestroy {
  isLoading = false;
  dataPeopleSub!: Subscription;
  peopleTest!:Person[];
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'username','birthDate'];
  dataSource = new MatTableDataSource<Person>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private dataService: DataStorageService) {
  }
  
  ngOnInit(): void {
    this.isLoading=true;
    this.setCurrentUser();
  }
  ngOnDestroy(): void {
    if(this.dataPeopleSub)this.dataPeopleSub.unsubscribe();
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.dataPeopleSub = this.dataService.getPeople(person.token).subscribe(
        (res) => {
          this.peopleTest = res;
        },
        (error) => {
          console.log(error.error);
        },()=>{
          this.isLoading=false;
          this.dataSource = new MatTableDataSource<Person>(this.peopleTest);
          this.dataSource.paginator = this.paginator;
          console.log(this.peopleTest);
        }
      );
    }
  }
}
