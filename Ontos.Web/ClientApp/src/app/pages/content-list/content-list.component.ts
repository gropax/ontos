import { Component, OnInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-content-list',
  templateUrl: './content-list.component.html',
  styleUrls: ['./content-list.component.css']
})
export class ContentListPage implements OnInit {

  faPlus = faPlus;

  constructor() { }

  ngOnInit() {
  }

}
