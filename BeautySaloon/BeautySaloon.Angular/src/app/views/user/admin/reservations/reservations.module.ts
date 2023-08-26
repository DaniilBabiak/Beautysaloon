import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReservationsRoutingModule } from './reservations-routing.module';
import { ReservationsComponent } from './reservations.component';
import { FormsModule } from '@angular/forms';
import { NgbdSortableHeader } from 'src/app/shared/classes/ngbdSortableHeader';


@NgModule({
  declarations: [
    ReservationsComponent,
    NgbdSortableHeader
  ],
  imports: [
    CommonModule,
    ReservationsRoutingModule,
    FormsModule
  ]
})
export class ReservationsModule { }
