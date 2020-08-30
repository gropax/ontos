import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { NgbPaginationModule, NgbToastModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ContentListPage } from './pages/content-list/content-list.component';
import { PaginationComponent } from './components/tables/pagination/pagination.component';
import { ContentTableComponent } from './components/tables/content-table/content-table.component';
import { NewContentFormComponent } from './components/forms/new-content-form/new-content-form.component';
import { NewContentModalComponent } from './components/modals/new-content-modal/new-content-modal.component';
import { NotificationComponent } from './components/notification/notification.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ContentListPage,
    PaginationComponent,
    NotificationComponent,
    ContentTableComponent,
    NewContentFormComponent,
    NewContentModalComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ContentListPage, pathMatch: 'full' },
      { path: 'content', component: ContentListPage },
    ]),
    NgbPaginationModule,
    NgbToastModule,
    NgbModalModule,
    FontAwesomeModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
