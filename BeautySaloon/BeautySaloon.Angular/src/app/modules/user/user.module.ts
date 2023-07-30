import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { AboutComponent } from './components/about/about.component';
import { ServicesComponent } from './components/services/services.component';
import { ContactsComponent } from './components/contacts/contacts.component';
import { WorksComponent } from './components/works/works.component';
import { ReviewsComponent } from './components/reviews/reviews.component';
import { AppointmentsComponent } from './components/appointments/appointments.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  declarations: [
    AdminDashboardComponent,
    HeaderComponent,
    FooterComponent,
    AboutComponent,
    ServicesComponent,
    ContactsComponent,
    WorksComponent,
    ReviewsComponent,
    AppointmentsComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
   
    
    FontAwesomeModule,
  ]
})
export class UserModule { }