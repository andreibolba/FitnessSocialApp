import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/services/auth.guard.service';
import { AdminDashboardComponent } from './admin/admin.dashboard/admin.dashboard.component';
import { AdministratorsComponent } from './admin/administrators/administrators.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { InternsComponent } from './admin/interns/interns.component';
import { TrainersComponent } from './admin/trainers/trainers.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { ForumComponent } from './forum/forum/forum.component';
import { HomeComponent } from './home/home.component';
import { MeetingComponent } from './shared/meeting/meeting.component';
import { TaskComponent } from './shared/task/task.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'administrators', component: AdministratorsComponent },
      { path: 'trainers', component: TrainersComponent },
      { path: 'interns', component: InternsComponent },
      { path: 'groups', component: GroupsComponent },
      { path: 'meetings', component: MeetingComponent },
      { path: 'tasks', component: TaskComponent },
      { path: 'forum', component: ForumComponent },
    ],
  },
  { path: '', component:AuthentificationComponent, pathMatch: 'full' },
  { path: 'authentification', component: AuthentificationComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
