import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent {
  displayedColumns: string[] = [
    'name',
    'trainer',
    'membersCount',
    'edit',
    'delete',
  ];

  hasTableValues:boolean=false;
  dataSource!: MatTableDataSource<Group>;
  groups!: Group[];
  dataGroupSub!: Subscription;
  private token: string = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private utils:UtilsService
  ) {}

  ngOnInit(): void {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.dataGroupSub = this.dataService
        .getGroups(person.token)
        .subscribe((data) => {
          this.dataSource = new MatTableDataSource(data);
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
    if (this.dataGroupSub) this.dataGroupSub.unsubscribe();
  }

  onDelete(obj: Group) {
    let id: number = obj.groupId;
  }

  onEdit(obj: Group) {
  }

  onAdd(){
  }


}

