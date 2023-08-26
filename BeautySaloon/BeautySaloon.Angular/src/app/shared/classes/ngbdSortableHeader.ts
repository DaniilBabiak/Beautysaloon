import { Directive, EventEmitter, Input, Output } from "@angular/core";

export const rotate: { [key: string]: SortDirection } = { 'asc': 'desc', 'desc': '', '': 'asc' };

@Directive({
    selector: 'th[sortable]',
    host: {
        '[class.asc]': 'direction === "asc"',
        '[class.desc]': 'direction === "desc"',
        '(click)': 'rotate()'
    }
})
export class NgbdSortableHeader<T> {

    @Input() sortable: SortColumn<T> = '';
    @Input() direction: SortDirection = '';
    @Output() sort = new EventEmitter<SortEvent<T>>();

    rotate() {
        this.direction = rotate[this.direction];
        this.sort.emit({ column: this.sortable, direction: this.direction });
    }
}

export type SortColumn<T> = keyof T | '';
export type SortDirection = 'asc' | 'desc' | '';
export interface SortEvent<T> {
    column: SortColumn<T>;
    direction: SortDirection;
  }