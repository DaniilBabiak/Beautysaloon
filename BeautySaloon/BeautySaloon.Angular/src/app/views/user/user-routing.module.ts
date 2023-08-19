import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from "./user.component";
import { AdminGuard } from 'src/app/shared/guards/admin.guard';
import { AuthGuard } from "../../shared/guards/auth.guard";

const routes: Routes = [
  {
    path: '', component: UserComponent,
    children: [
      { path: 'about', loadChildren: () => import('./about/about.module').then(m => m.AboutModule) },
      { path: 'services', loadChildren: () => import('./service/service.module').then(m => m.ServiceModule) },
      { path: 'reviews', loadChildren: () => import('./reviews/reviews.module').then(m => m.ReviewsModule) },
      { path: 'works', loadChildren: () => import('./works/works.module').then(m => m.WorksModule) },
      { path: 'contacts', loadChildren: () => import('./contact/contact.module').then(m => m.ContactModule) },
      { path: 'appointments', loadChildren: () => import('./appointments/appointments.module').then(m => m.AppointmentsModule) },
      { path: 'profile', loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule) },
      { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), canActivate: [AdminGuard] },
      { path: 'reservations', loadChildren: () => import('./reservation/reservation.module').then(m => m.ReservationModule),canActivate: [AuthGuard], },
      { path: '**', redirectTo: 'about' },
    ]
  },



];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
