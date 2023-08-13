import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddServiceRoutingModule } from './add-service-routing.module';
import { AddServiceComponent } from './add-service.component';
import {FormsModule} from "@angular/forms";
import {SharedModule} from "../../../../shared/shared.module";


@NgModule({
  declarations: [
    AddServiceComponent
  ],
    imports: [
        CommonModule,
        AddServiceRoutingModule,
        FormsModule,
        SharedModule,
    ]
})
export class AddServiceModule { }
