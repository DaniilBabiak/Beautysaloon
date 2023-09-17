import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  { path: 'category', loadChildren: () => import('./category/category.module').then(m => m.CategoryModule) },
  { path: 'best-work', loadChildren: () => import('./best-work/best-work.module').then(m => m.BestWorkModule) },
  { path: 'service', loadChildren: () => import('./service/service.module').then(m => m.ServiceModule) },
  { path: 'master', loadChildren: () => import('./master/master.module').then(m => m.MasterModule) },
  { path: 'reservations', loadChildren: () => import('./reservations/reservations.module').then(m => m.ReservationsModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
