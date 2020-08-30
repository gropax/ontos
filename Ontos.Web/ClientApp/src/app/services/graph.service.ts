import { Injectable } from '@angular/core';
import { PaginationParams, Paginated } from '../models/pagination';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GraphService {

  constructor(private http: HttpClient) { }

  public getContentList(params: PaginationParams<Content>) {
    return this.getPaginated(`api/graph`, params);
  }

  protected getPaginated<T>(url: string, pageParams: PaginationParams<T>,
    otherParams: { [param: string]: string } = {}): Observable<Paginated<T>>
  {
    return this.http.get<Paginated<T>>(url, {
      params: { ...pageParams.toStringParams(), ...otherParams },
    });
  }

}
