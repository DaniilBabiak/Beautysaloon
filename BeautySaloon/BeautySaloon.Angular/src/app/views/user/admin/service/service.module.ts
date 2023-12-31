import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddServiceRoutingModule } from './service-routing.module';
import { NgbDropdownModule, NgbTimeAdapter, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from "@angular/forms";
import { NgbTimeStringAdapter } from 'src/app/shared/helpers/ngb-time-string-adapter';
import { ServiceComponent } from './service.component';
import { ServiceDetailsComponent } from './service-details/service-details.component';
@NgModule({
  declarations: [
    ServiceComponent,
    ServiceDetailsComponent
  ],
  imports: [
    CommonModule,
    AddServiceRoutingModule,
    NgbDropdownModule,
    FormsModule,
    NgbTimepickerModule,
  ],
  providers: [NgbTimeStringAdapter]
})
export class ServiceModule { }
