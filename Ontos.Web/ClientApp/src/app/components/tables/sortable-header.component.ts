import { Directive, Input, EventEmitter, Output } from '@angular/core';
import { SortDirection, SortColumn, SortEvent } from '../../models/pagination';


const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };

@Directive({
  selector: 'th[sortable]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()'
  }
})
export class SortableHeader<T> {

  @Input() sortable: SortColumn<T> = '';
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent<T>>();

  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({column: this.sortable, direction: this.direction});
  }
}
