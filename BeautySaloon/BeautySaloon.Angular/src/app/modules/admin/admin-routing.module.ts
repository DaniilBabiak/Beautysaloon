import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { MainComponent } from './components/main/main.component';
import { AboutComponent } from './components/about/about.component';
import { ContactsComponent } from './components/contacts/contacts.component';
import { ReviewsComponent } from './components/reviews/reviews.component';
import { AppointmentsComponent } from './components/appointments/appointments.component';
import { WorksComponent } from './components/works/works.component';
import { ServicesComponent } from './components/services/services.component';

const routes: Routes = [
  {path:'', component:AdminDashboardComponent, children:[
    {path:'main', component:MainComponent},
    {path:'about', component:AboutComponent},
    {path:'contacts', component:ContactsComponent},
    {path:'reviews', component:ReviewsComponent},
    {path:'appointments', component:AppointmentsComponent},
    {path:'works', component:WorksComponent},
    {path:'services', component:ServicesComponent},
    {path:'**', redirectTo:'/admin/main', pathMatch:'full'},
    {path:'', redirectTo: '/admin/main', pathMatch:'full'},
  ]},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
