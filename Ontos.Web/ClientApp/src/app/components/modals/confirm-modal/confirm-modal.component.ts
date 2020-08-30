import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { faFistRaised } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.css']
})
export class ConfirmModalComponent implements OnInit {

  @Input() message: string;
  @Output() confirm = new EventEmitter();
  @Output() close = new EventEmitter();

  confirmIcon = faFistRaised;

  ngOnInit() {
  }

}
