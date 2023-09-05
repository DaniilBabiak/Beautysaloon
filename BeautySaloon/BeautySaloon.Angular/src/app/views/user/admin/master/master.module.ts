import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MasterRoutingModule } from './master-routing.module';
import { MasterComponent } from './master.component';
import { MasterDetailsComponent } from './details/master-details.component';
import { FormsModule } from '@angular/forms';
import { NgbDropdownModule, NgbModule, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbTimeStringAdapter } from 'src/app/shared/helpers/ngb-time-string-adapter';
import { ScheduleComponent } from './schedule/schedule.component';


@NgModule({
  declarations: [
    MasterComponent,
    MasterDetailsComponent,
    ScheduleComponent,
    
  ],
  imports: [
    CommonModule,
    MasterRoutingModule,
    FormsModule,
    NgbModule,
    NgbDropdownModule,
    NgbTimepickerModule,
  ],
  providers: [NgbTimeStringAdapter]
})
export class MasterModule { }
