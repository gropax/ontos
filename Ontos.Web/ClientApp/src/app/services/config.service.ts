import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  public dateFormat: string = 'dd/MM/yyyy';
  public datetimeFormat: string = 'dd/MM/yyyy HH:mm:ss';
  public defaultPageSize: number = 25;

  constructor() { }
}
