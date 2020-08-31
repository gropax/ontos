import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { NgbPaginationModule, NgbToastModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TimeagoModule } from 'ngx-timeago';
import { MarkdownModule } from 'ngx-markdown';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { PageListPage } from './pages/page-list/page-list.component';
import { PaginationComponent } from './components/tables/pagination/pagination.component';
import { PageTableComponent } from './components/tables/page-table/page-table.component';
import { NewPageFormComponent } from './components/forms/new-page-form/new-page-form.component';
import { NewPageModalComponent } from './components/modals/new-page-modal/new-page-modal.component';
import { NotificationComponent } from './components/notification/notification.component';
import { SortableHeader } from './components/tables/sortable-header.component';
import { PagePage } from './pages/page/page.component';
import { PageEditPage } from './pages/page-edit/page-edit.component';
import { EditPageFormComponent } from './components/forms/edit-page-form/edit-page-form.component';
import { ConfirmModalComponent } from './components/modals/confirm-modal/confirm-modal.component';
import { HoverClassDirective } from './directives/hover-class.directive';
import { LanguageSelectorComponent } from './components/forms/language-selector/language-selector.component';
import { PageTitleComponent } from './pages/page/page-title/page-title.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SortableHeader,
    PageListPage,
    PaginationComponent,
    NotificationComponent,
    PageTableComponent,
    NewPageFormComponent,
    NewPageModalComponent,
    PagePage,
    PageEditPage,
    EditPageFormComponent,
    ConfirmModalComponent,
    HoverClassDirective,
    LanguageSelectorComponent,
    PageTitleComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: PageListPage, pathMatch: 'full' },
      { path: 'pages', component: PageListPage },
      { path: 'pages/:id', component: PagePage },
      { path: 'pages/:id/edit', component: PageEditPage },
    ]),
    NgbPaginationModule,
    NgbToastModule,
    NgbModalModule,
    FontAwesomeModule,
    TimeagoModule.forRoot(),
    MarkdownModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
