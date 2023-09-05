import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ReservationService } from 'src/app/shared/services/reservation.service';
import { NgbdSortableHeader, SortEvent } from 'src/app/shared/classes/ngbdSortableHeader';
import { ReservationModel } from 'src/app/shared/models/resrvation/reservation-model';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  reservations: ReservationModel[] = [];
  sortBy: string = 'DateTime';
  currentPage: number = 1;
  pageSize: number = 5;
  totalPages: number = 1;

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader<ReservationModel>> | null = null;

  constructor(private auth: AuthService, private reservationService: ReservationService) {

  }

  ngOnInit(): void {
    this.auth.loadUser()?.then(() => {
      this.loadReservations();
    });
  };

  loadReservations(): void {
    this.reservationService.getAllReservations(this.currentPage, this.pageSize, this.sortBy).subscribe(result => {
      this.reservations = result.pageItems;
      this.totalPages = result.totalPages;
      console.log(this.reservations);
    });
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadReservations();
  }

  onPageSizeChange() {
    this.currentPage = 1; // Сбросить текущую страницу при изменении размера страницы
    this.loadReservations();
  }

  onSortChange(event: SortEvent<ReservationModel>) {
    if (this.headers) {
      this.headers.forEach(header => {
        if (header.sortable !== event.column) {
          header.direction = '';
        }
      });

      if (event.direction === '' || event.column === '') {
        this.sortBy = 'DateTime';
      } else {
        this.sortBy = event.column;
        if (event.direction === 'desc') {
          this.sortBy = `-${this.sortBy}`;
        }
      }
      this.currentPage = 1;
      this.loadReservations();
    }
  }
}