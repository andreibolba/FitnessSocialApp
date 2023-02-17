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
export class AdministratorsComponent {
  displayedColumns: string[] = ['firstName', 'lastName', 'email',  'username', 'birthDate'];
  dataSource!: MatTableDataSource<Person>;
  people!: Person[];
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private service: DataStorageService) {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
    this.service.getPeople(person.token).subscribe((data) => {
      this.people = data;
      this.dataSource = new MatTableDataSource(this.people.filter(p=>p.status=='Admin'));
      this.dataSource.paginator = this.paginator;
    });
  }
  }
}

