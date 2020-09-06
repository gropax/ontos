import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { NgbPaginationModule, NgbToastModule, NgbModalModule, NgbTooltipModule, NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
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
import { PageDenominationsPage } from './pages/page-denominations/page-denominations.component';
import { ReferenceTableComponent } from './components/tables/reference-table/reference-table.component';
import { ExpressionFormComponent } from './components/forms/expression-form/expression-form.component';
import { ReferenceInlineFormComponent } from './components/forms/reference-inline-form/reference-inline-form.component';
import { DeleteReferenceButtonComponent } from './pages/page-denominations/delete-reference-button/delete-reference-button.component';
import { PageResolver } from './resolvers/page.resolver';
import { ReferencesResolver } from './resolvers/references.resolver';
import { PageRelationsComponent } from './pages/page/page-relations/page-relations.component';
import { PageRelationComponent } from './pages/page/page-relations/page-relation/page-relation.component';

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
    PageDenominationsPage,
    ReferenceTableComponent,
    ExpressionFormComponent,
    ReferenceInlineFormComponent,
    DeleteReferenceButtonComponent,
    PageRelationsComponent,
    PageRelationComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: PageListPage, pathMatch: 'full' },
      { path: 'pages', component: PageListPage, data: { animation: 'PageList' }  },
      {
        path: 'pages/:id',
        component: PagePage,
        data: { animation: 'Page' },
        resolve: { page: PageResolver },
      },
      {
        path: 'pages/:id/edit',
        component: PageEditPage,
        data: { animation: 'PageEdit' },
        resolve: { page: PageResolver },
      },
      {
        path: 'pages/:id/denominations',
        component: PageDenominationsPage,
        data: { animation: 'PageDenominations' },
        resolve: {
          page: PageResolver,
          references: ReferencesResolver,
        },
      },
    ]),
    NgbPaginationModule,
    NgbToastModule,
    NgbModalModule,
    NgbTooltipModule,
    NgbCollapseModule,
    FontAwesomeModule,
    TimeagoModule.forRoot(),
    MarkdownModule.forRoot(),
  ],
  providers: [
    PageResolver,
    ReferencesResolver,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
