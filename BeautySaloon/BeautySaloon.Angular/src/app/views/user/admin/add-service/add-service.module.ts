import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddServiceRoutingModule } from './add-service-routing.module';
import { AddServiceComponent } from './add-service.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from "@angular/forms";
@NgModule({
  declarations: [
    AddServiceComponent
  ],
  imports: [
    CommonModule,
    AddServiceRoutingModule,
    NgbDropdownModule,
    FormsModule,
  ]
})
export class AddServiceModule { }
