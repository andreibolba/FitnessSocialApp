import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataStorageService } from 'src/services/data-storage.service';
import { Subscription } from 'rxjs';
import { Person } from 'src/model/person.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
})
export class AdministrationComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'firstName',
    'lastName',
    'email',
    'username',
    'birthDate',
    'edit',
    'delete'
  ];
  dataSource!: MatTableDataSource<Person>;
  people!: Person[];
  dataPeopleSub!: Subscription;
  status: string = '';
  icon: string = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private service: DataStorageService, private router: Router) {}

  ngOnInit(): void {
    switch (this.router.url) {
      case '/dashboard/administrators':
        this.status = 'Admin';
        this.icon = 'uil uil-user-md';
        break;
      case '/dashboard/interns':
        this.status = 'Intern';
        this.icon = 'uil uil-book-reader';
        break;
      case '/dashboard/trainers':
        this.status = 'Trainer';
        this.icon = 'uil uil-user';
        break;
      default:
        break;
    }

    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.dataPeopleSub = this.service
        .getPeople(person.token)
        .subscribe((data) => {
          this.people = data;
          this.dataSource = new MatTableDataSource(
            this.people.filter((p) => p.status == this.status)
          );
          this.dataSource.paginator = this.paginator;
        });
    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
  }

  onDelete(obj:Person){
    console.log(obj);
  }

  onEdit(obj:Person){
    console.log(obj);
  }
}
