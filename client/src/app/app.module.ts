import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminDashboardComponent } from './admin/admin.dashboard/admin.dashboard.component';
import { TrainerDashboardComponent } from './trainer/trainer.dashboard/trainer.dashboard.component';
import { InternDashboardComponent } from './intern/intern.dashboard/intern.dashboard.component';
import { ForumComponent } from './forum/forum/forum.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { TaskComponent } from './shared/task/task.component';
import { MeetingComponent } from './shared/meeting/meeting.component';
import { ProfileComponent } from './shared/profile/profile.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NoteComponent } from './shared/note/note.component';
import { FeedbackComponent } from './shared/feedback/feedback.component';
import { ChallangeComponent } from './shared/challange/challange.component';
import { TestComponent } from './shared/tests/test/test.component';
import { MyGroupComponent } from './intern/my.group/my.group.component';
import { MyGroupsComponent } from './trainer/my.groups/my.groups.component';
import { ToastrModule } from 'ngx-toastr';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AdministrationComponent } from './admin/administration/administration.component';
import { CreateEditDialogComponent } from './admin/create-edit-dialog/create-edit-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { EditGroupDialogComponent } from './admin/edit-group-dialog/edit-group-dialog.component';
import { EditGroupMembersDialogComponent } from './admin/edit-group-members-dialog/edit-group-members-dialog.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ErrorComponent } from './error/error.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { QuestionsComponent } from './shared/questions/question/question.component';
import { EditDialogComponent } from './shared/questions/edit-dialog/edit-dialog.component';
import { EditTestsComponent } from './shared/tests/edit-tests/edit-tests.component';

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
    GroupsComponent,
    TaskComponent,
    ProfileComponent,
    NoteComponent,
    FeedbackComponent,
    ChallangeComponent,
    TestComponent,
    MyGroupComponent,
    MyGroupsComponent,
    AdministrationComponent,
    CreateEditDialogComponent,
    EditGroupDialogComponent,
    EditGroupMembersDialogComponent,
    ErrorComponent,
    QuestionsComponent,
    EditDialogComponent,
    EditTestsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatDatepickerModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatCheckboxModule,
    MatExpansionModule,
    ToastrModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
