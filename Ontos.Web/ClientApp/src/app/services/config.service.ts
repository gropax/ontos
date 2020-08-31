import { Injectable } from '@angular/core';


export class Language {
  constructor(
    public code: string,
    public name: string) {
  }
}


@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  public dateFormat: string = 'dd/MM/yyyy';
  public datetimeFormat: string = 'dd/MM/yyyy HH:mm:ss';
  public defaultPageSize: number = 25;

  public languages = [
    new Language("fr", "French"),
    new Language("en", "English"),
  ];

  public preferredLanguage = "fr";

  constructor() { }
}
