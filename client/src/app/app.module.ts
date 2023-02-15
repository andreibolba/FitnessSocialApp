import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminDashboardComponent } from './admin/admin.dashboard/admin.dashboard.component';
import { TrainerDashboardComponent } from './trainer/trainer.dashboard/trainer.dashboard.component';
import { InternDashboardComponent } from './intern/intern.dashboard/intern.dashboard.component';
import { ForumComponent } from './forum/forum/forum.component';
import { AdministratorsComponent } from './admin/administrators/administrators.component';
import { TrainersComponent } from './admin/trainers/trainers.component';
import { InternsComponent } from './admin/interns/interns.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { TaskComponent } from './shared/task/task.component';
import { MeetingComponent } from './shared/meeting/meeting.component';
import { ProfileComponent } from './shared/profile/profile.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NoteComponent } from './shared/note/note.component';
import { FeedbackComponent } from './shared/feedback/feedback.component';
import { ChallangeComponent } from './shared/challange/challange.component';
import { TestComponent } from './shared/test/test.component';
import { MyGroupComponent } from './intern/my.group/my.group.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    AuthentificationComponent,
    HomeComponent,
    AdminDashboardComponent,
    TrainerDashboardComponent,
    InternDashboardComponent,
    ForumComponent,
    MeetingComponent,
    AdministratorsComponent,
    TrainersComponent,
    InternsComponent,
    GroupsComponent,
    TaskComponent,
    ProfileComponent,
    NoteComponent,
    FeedbackComponent,
    ChallangeComponent,
    TestComponent,
    MyGroupComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatDatepickerModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
