import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { EditGroupDialogComponent } from '../edit-group-dialog/edit-group-dialog.component';
import { EditGroupMembersDialogComponent } from '../edit-group-members-dialog/edit-group-members-dialog.component';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent {
  displayedColumns: string[] = [];

  hasTableValues:boolean=false;
  dataSource!: MatTableDataSource<Group>;
  groups!: Group[];
  dataGroupSub!: Subscription;
  peopleSub!: Subscription;
  isAdmin=false;
  private token: string = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private utils:UtilsService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.peopleSub=this.dataService.personData.getPerson(person.username,this.token).subscribe((res)=>{
        this.isAdmin=res.status=="Admin";
        this.dataGroupSub = this.dataService
          .getGroups(person.token)
          .subscribe((data) => {
            if(this.isAdmin==false)
              {data=data.filter(g=>g.trainerId==res.personId);
                this.displayedColumns=[
                  'groupName',
                  'trainer',
                  'membersCount',
                  'seeInfo'
                ];
              }else{
                this.displayedColumns=[
                  'groupName',
                  'trainer',
                  'membersCount',
                  'editMembers',
                  'edit',
                  'delete',
                ];
              }
            this.dataSource = new MatTableDataSource(data);
            this.dataSource.paginator = this.paginator;
            if(this.dataSource.data.length==0){
              this.hasTableValues=false;
            }else{
              this.hasTableValues = true;
            }
          });
      })

    }
  }

  ngOnDestroy(): void {
    if (this.dataGroupSub) this.dataGroupSub.unsubscribe();
  }

  onDelete(obj: Group) {
    let id: number = +obj.groupId;
    this.dataService.deleteGroup(this.token,id).subscribe(
      () => {
        this.toastr.success('Delete was done successfully!');
        if(this.dataSource.data.length==1){
          this.dataSource=new MatTableDataSource<Group>();
          this.hasTableValues=false;
        }else{
        this.hasTableValues = true;
        }
      },
      (error) => {
        console.log(error);
        this.toastr.error('Delete was not made! An error has occured!');
      }
    );
  }

  onEdit(obj: Group) {
    this.utils.groupToEdit.next(obj);
    this.openDialog(1);
  }

  onEditMembers(obj: Group) {
    this.utils.groupToEdit.next(obj);
    this.openDialog(2);
  }

  onAdd(){
    this.utils.groupToEdit.next(null);
    this.openDialog(1);
  }

  openDialog(op:number) {
    const dialogRef = op==1? this.dialog.open(EditGroupDialogComponent): this.dialog.open(EditGroupMembersDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  seeGroupDetails(group:Group){
    this.utils.isFromGroupDashboard.next(true);
    this.router.navigate(['main/'+group.groupId], { relativeTo: this.route.parent });
  }

}

