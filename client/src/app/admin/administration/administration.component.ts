import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { DataStorageService } from 'src/services/data-storage.service';
import { Subscription } from 'rxjs';
import { Person } from 'src/model/person.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {MatDialog} from '@angular/material/dialog';
import { CreateEditDialogComponent } from '../create-edit-dialog/create-edit-dialog.component';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
})
export class AdministrationComponent implements OnInit, OnDestroy, OnChanges {
  displayedColumns: string[] = [
    'firstName',
    'lastName',
    'email',
    'username',
    'birthDate',
    'edit'
  ];
  hasTableValues:boolean=false;
  dataSource!: MatTableDataSource<Person>;
  people!: Person[];
  dataPeopleSub!: Subscription;
  status: string = '';
  icon: string = '';
  private token: string = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private router: Router,
    private dialog: MatDialog,
    private utils:UtilsService
  ) {}
  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
  }

  ngOnInit(): void {
    console.log(new Date());
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
      this.token = person.token;
      this.dataPeopleSub = this.dataService
        .getPeople(person.token)
        .subscribe((data) => {
          this.people = data;
          this.dataSource = new MatTableDataSource(
            this.people.filter((p) => p.status == this.status)
          );
          this.dataSource.paginator = this.paginator;
          if(this.dataSource.data.length==0){
            this.hasTableValues=false;
          }else{
            this.hasTableValues = true;
          }
        });
    }
  }

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
  }

  onDelete(obj: Person) {
    let id: number = +obj.personId;
    this.dataService.deletePerson(id, this.token).subscribe(
      () => {
        this.toastr.success('Delete was done successfully!');
        if(this.dataSource.data.length==1){
          this.dataSource=new MatTableDataSource<Person>();
          this.hasTableValues=false;
        }else{
        let index = this.dataSource.data.findIndex((p) => p.personId == id);
        this.dataSource.data = this.dataSource.data.splice(index, 1);
        this.hasTableValues = true;
        }
      },
      () => {
        this.toastr.error('Delete was not! An error has occured!');
      }
    );
  }

  onEdit(obj: Person) {
    this.utils.userToEdit.next(obj);
    this.openDialog();
  }

  onAdd(){
    this.utils.userToEdit.next(null);
    this.openDialog();
  }

  openDialog() {
    const dialogRef = this.dialog.open(CreateEditDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}
