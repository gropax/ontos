import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ConfigService, Language } from '../../../services/config.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit {

  private languages: Language[];
  public selectedLanguage: string;

  @Output() change = new EventEmitter<string>();

  constructor(
    private configService: ConfigService) {
  }

  ngOnInit() {
    this.languages = this.configService.languages;
    this.selectedLanguage = this.languages[0].code;
  }

}
