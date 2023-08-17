import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddServiceRoutingModule } from './add-service-routing.module';
import { AddServiceComponent } from './add-service.component';
import { NgbDropdownModule, NgbTimeAdapter, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from "@angular/forms";
import { NgbTimeStringAdapter } from 'src/app/shared/helpers/ngb-time-string-adapter';
@NgModule({
  declarations: [
    AddServiceComponent
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
export class AddServiceModule { }
