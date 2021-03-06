import { Injectable } from '@angular/core';
import { PaginationParams, Paginated } from '../models/pagination';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Page, NewPage, UpdatePage, Reference, NewReference, Relation, NewRelation, RelatedPage, PageSearch, PageSearchResult, NewRelatedPage } from '../models/graph';

@Injectable({
  providedIn: 'root'
})
export class GraphService {

  constructor(private http: HttpClient) { }

  public getPaginatedPages(params: PaginationParams<Page>) {
    return this.getPaginated(`api/pages`, params);
  }

  public getPage(id: string | number) {
    return this.http.get<Page>(`api/pages/${id}`);
  }

  public searchPages(params: PageSearch) {
    return this.http.post<PageSearchResult[]>(`api/pages/search`, params);
  }

  public createPage(params: NewPage) {
    return this.http.post<Page>(`api/pages`, params);
  }

  public updatePage(params: UpdatePage) {
    return this.http.put<Page>(`api/pages`, params);
  }

  public deletePage(id: string | number) {
    return this.http.delete(`api/pages/${id}`);
  }

  public getReferences(id: string | number) {
    return this.http.get<Reference[]>(`api/pages/${id}/references`);
  }

  public createReference(params: NewReference) {
    return this.http.post<Reference>(`api/references`, params);
  }

  public deleteReference(id: string | number) {
    return this.http.delete(`api/references/${id}`);
  }

  public getRelatedPages(id: string | number) {
    return this.http.get<RelatedPage[]>(`api/pages/${id}/relations`);
  }

  public createRelation(params: NewRelation) {
    return this.http.post<Relation>(`api/relations`, params.toParams());
  }

  public createRelatedPage(id: string | number, params: NewRelatedPage) {
    return this.http.post<RelatedPage>(`api/pages/${id}/relations`, params.toParams());
  }

  public deleteRelation(id: string | number) {
    return this.http.delete(`api/relations/${id}`);
  }

  protected getPaginated<T>(url: string, pageParams: PaginationParams<T>,
    otherParams: { [param: string]: string } = {}): Observable<Paginated<T>>
  {
    return this.http.get<Paginated<T>>(url, {
      params: { ...pageParams.toStringParams(), ...otherParams },
    });
  }

}
