import { Injectable } from '@angular/core';
import { PaginationParams, Paginated } from '../models/pagination';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Page, NewPage } from '../models/graph';

@Injectable({
  providedIn: 'root'
})
export class GraphService {

  constructor(private http: HttpClient) { }

  public getContentList(params: PaginationParams<Page>) {
    return this.getPaginated(`api/pages`, params);
  }

  public getPage(id: string) {
    return this.http.get<Page>(`api/pages/${id}`);
  }

  public createPage(params: NewPage) {
    return this.http.post<Page>(`api/pages`, params);
  }

  protected getPaginated<T>(url: string, pageParams: PaginationParams<T>,
    otherParams: { [param: string]: string } = {}): Observable<Paginated<T>>
  {
    return this.http.get<Paginated<T>>(url, {
      params: { ...pageParams.toStringParams(), ...otherParams },
    });
  }

}
