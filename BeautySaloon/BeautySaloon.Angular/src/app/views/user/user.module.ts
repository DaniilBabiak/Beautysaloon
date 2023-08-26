import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { SharedModule } from "../../shared/shared.module";
import { ReservationComponent } from './add-reservation/add-reservation.component';
import { NgbDropdownModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
@NgModule({
    declarations: [
        UserComponent,
        ReservationComponent
    ],
    imports: [
        FormsModule,
        CommonModule,
        UserRoutingModule,
        SharedModule,
        NgbModule,
        NgbDropdownModule,

    ]
})
export class UserModule {
}
