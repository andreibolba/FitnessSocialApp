import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/services/auth.guard.service';
import { AdministrationComponent } from './admin/administration/administration.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { ErrorComponent } from './error/error.component';
import { ForumComponent } from './forum/forum/forum.component';
import { HomeComponent } from './home/home.component';
import { FeedbackComponent } from './shared/feedback/feedback.component';
import { DashboardComponent } from './shared/groups/groups-dashboard/dashboard/dashboard.component';
import { MeetingComponent } from './shared/meetings/meeting/meeting.component';
import { NoteComponent } from './shared/notes/note/note.component';
import { ProfileComponent } from './shared/profile/profile.component';
import { QuestionsComponent } from './shared/questions/question/question.component';
import { TaskComponent } from './shared/task/task.component';
import { TestComponent } from './shared/tests/test/test.component';
import { ChatComponent } from './chat-system/chat/chat.component';
import { ChallengesComponent } from './shared/challenge/challenges/challenges.component';
import { RankingsComponent } from './shared/challenge/rankings/rankings.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'administrators', component: AdministrationComponent },
      { path: 'trainers', component: AdministrationComponent },
      { path: 'interns', component: AdministrationComponent },
      { path: 'groups', component: GroupsComponent },
      { path: 'meetings', component: MeetingComponent },
      { path: 'tasks', component: TaskComponent },
      { path: 'forum', component: ForumComponent },
      { path: 'tests', component: TestComponent },
      {
        path: 'challenges', component: ChallengesComponent, children: [{
          path: 'ranking', component: RankingsComponent
        }]
      },
      { path: 'feedback', component: FeedbackComponent },
      { path: 'notes', component: NoteComponent },
      { path: 'mygroups', component: GroupsComponent },
      { path: 'chat', component: ChatComponent },
      {
        path: 'main',
        children: [{ path: ':id', component: DashboardComponent }],
      },
      { path: 'profile', component: ProfileComponent },
      { path: 'questions', component: QuestionsComponent },
    ],
  },
  {
    path: 'recovery/:linkid',
    component: AuthentificationComponent,
  },
  { path: '', component: AuthentificationComponent, pathMatch: 'full' },
  { path: 'authentification', component: AuthentificationComponent },
  { path: 'error', component: ErrorComponent },
  { path: '**', redirectTo: 'error' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
