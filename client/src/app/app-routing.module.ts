import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/services/auth.guard.service';;
import { AdministratorsComponent } from './admin/administrators/administrators.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { InternsComponent } from './admin/interns/interns.component';
import { TrainersComponent } from './admin/trainers/trainers.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { ForumComponent } from './forum/forum/forum.component';
import { HomeComponent } from './home/home.component';
import { MyGroupComponent } from './intern/my.group/my.group.component';
import { ChallangeComponent } from './shared/challange/challange.component';
import { FeedbackComponent } from './shared/feedback/feedback.component';
import { MeetingComponent } from './shared/meeting/meeting.component';
import { NoteComponent } from './shared/note/note.component';
import { ProfileComponent } from './shared/profile/profile.component';
import { TaskComponent } from './shared/task/task.component';
import { TestComponent } from './shared/test/test.component';
import { MyGroupsComponent } from './trainer/my.groups/my.groups.component';

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
      { path: 'tests', component: TestComponent },
      { path: 'challanges', component: ChallangeComponent },
      { path: 'feedback', component: FeedbackComponent },
      { path: 'notes', component: NoteComponent },
      { path: 'mygroup', component: MyGroupComponent },
      { path: 'mygroups', component: MyGroupsComponent },
      { path: 'profile', component: ProfileComponent },
    ],
  },
  {
    path: 'recovery/:linkid',
    component: AuthentificationComponent
  },
  { path: '', component:AuthentificationComponent, pathMatch: 'full' },
  { path: 'authentification', component: AuthentificationComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
