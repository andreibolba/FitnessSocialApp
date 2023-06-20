import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminDashboardComponent } from './admin/admin.dashboard/admin.dashboard.component';
import { TrainerDashboardComponent } from './trainer/trainer.dashboard/trainer.dashboard.component';
import { InternDashboardComponent } from './intern/intern.dashboard/intern.dashboard.component';
import { ForumComponent } from './forum/forum/forum.component';
import { GroupsComponent } from './admin/groups/groups.component';
import { TaskComponent } from './shared/tasks/task/task.component';
import { MeetingComponent } from './shared/meetings/meeting/meeting.component';
import { ProfileComponent } from './shared/profile/profile.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NoteComponent } from './shared/notes/note/note.component';
import { FeedbackComponent } from './shared/feedbacks/feedback/feedback.component';
import { TestComponent } from './shared/tests/test/test.component';
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
import { EditIntersComponent } from './shared/tests/edit-inters/edit-inters.component';
import { EditGroupsComponent } from './shared/tests/edit-groups/edit-groups.component';
import { EditMeetingDialogComponent } from './shared/meetings/edit-meeting-dialog/edit-meeting-dialog.component';
import { SeeMeetingParticipantsComponent } from './shared/meetings/see-meeting-participants/see-meeting-participants.component';
import { StartTestComponent } from './shared/tests/start-test/start-test.component';
import { SeeAllResultsComponent } from './shared/tests/see-all-results/see-all-results.component';
import {MatListModule} from '@angular/material/list';
import { SeePostComponent } from './forum/see-post/see-post.component';
import { AddEditPostComponent } from './forum/add-edit-post/add-edit-post.component';
import { AddEditCommentComponent } from './forum/add-edit-comment/add-edit-comment.component';
import { GroupComponent } from './shared/groups/groups-dashboard/group/group.component';
import { DashboardComponent } from './shared/groups/groups-dashboard/dashboard/dashboard.component';
import { MatTabsModule } from '@angular/material/tabs';
import { ParticipantsComponent } from './shared/groups/groups-dashboard/participants/participants.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import { ChatComponent } from './chat-system/chat/chat.component';
import { ChatMessagesComponent } from './chat-system/chat-messages/chat-messages.component';
import { GroupChatMessageComponent } from './chat-system/group-chat-message/group-chat-message.component';
import { AllChatsComponent } from './chat-system/all-chats/all-chats.component';
import { AddEditGroupChatComponent } from './chat-system/add-edit-group-chat/add-edit-group-chat.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { GroupChatDetailsComponent } from './chat-system/group-chat-details/group-chat-details.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { AddEditNoteComponent } from './shared/notes/add-edit-note/add-edit-note.component';
import { ChallengesComponent } from './shared/challenge/challenges/challenges.component';
import { AddEditChallengeComponent } from './shared/challenge/add-edit-challenge/add-edit-challenge.component';
import { SeeSolutionsComponent } from './shared/challenge/see-solutions/see-solutions.component';
import { AddEditSolutionComponent } from './shared/challenge/add-edit-solution/add-edit-solution.component';
import { SolutionPointsComponent } from './shared/challenge/solution-points/solution-points.component';
import { Ng5SliderModule } from 'ng5-slider';
import { RankingsComponent } from './shared/challenge/rankings/rankings.component';
import { AddEditTaskComponent } from './shared/tasks/add-edit-task/add-edit-task.component';
import { AssignTaskComponent } from './shared/tasks/assign-task/assign-task.component';
import { UploadTaskSolutionComponent } from './shared/tasks/upload-task-solution/upload-task-solution.component';
import { SeeAllTaskSolutionsComponent } from './shared/tasks/see-all-task-solutions/see-all-task-solutions.component';
import { AddEditFeedbackComponent } from './shared/feedbacks/add-edit-feedback/add-edit-feedback.component';
import { UploadPhotoComponent } from './shared/upload-photo/upload-photo.component';
import { SeeFeedbackComponent } from './shared/feedbacks/see-feedback/see-feedback.component';

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
    TestComponent,
    AdministrationComponent,
    CreateEditDialogComponent,
    EditGroupDialogComponent,
    EditGroupMembersDialogComponent,
    ErrorComponent,
    QuestionsComponent,
    EditDialogComponent,
    EditTestsComponent,
    EditIntersComponent,
    EditGroupsComponent,
    EditMeetingDialogComponent,
    SeeMeetingParticipantsComponent,
    StartTestComponent,
    SeeAllResultsComponent,
    SeePostComponent,
    AddEditPostComponent,
    AddEditCommentComponent,
    GroupComponent,
    DashboardComponent,
    ParticipantsComponent,
    ChatComponent,
    ChatMessagesComponent,
    GroupChatMessageComponent,
    AllChatsComponent,
    AddEditGroupChatComponent,
    GroupChatDetailsComponent,
    AddEditNoteComponent,
    ChallengesComponent,
    AddEditChallengeComponent,
    SeeSolutionsComponent,
    AddEditSolutionComponent,
    SolutionPointsComponent,
    RankingsComponent,
    AddEditTaskComponent,
    AssignTaskComponent,
    UploadTaskSolutionComponent,
    SeeAllTaskSolutionsComponent,
    AddEditFeedbackComponent,
    UploadPhotoComponent,
    SeeFeedbackComponent
  ],
  imports: [
    Ng2SearchPipeModule,
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
    MatListModule,
    ToastrModule.forRoot(),
    MatTabsModule,
    MatFormFieldModule,
    ToastrModule.forRoot(),
    NgxMatSelectSearchModule ,
    Ng5SliderModule

  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}

