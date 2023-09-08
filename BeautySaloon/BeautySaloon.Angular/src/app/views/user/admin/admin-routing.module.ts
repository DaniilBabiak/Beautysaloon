import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  { path: 'add-category', loadChildren: () => import('./add-category/add-category.module').then(m => m.AddCategoryModule) },
  { path: 'add-best-works', loadChildren: () => import('./add-best-works/add-best-works.module').then(m => m.AddBestWorksModule) },
  { path: 'service', loadChildren: () => import('./service/service.module').then(m => m.ServiceModule) },
  { path: 'master', loadChildren: () => import('./master/master.module').then(m => m.MasterModule) },
  { path: 'reservations', loadChildren: () => import('./reservations/reservations.module').then(m => m.ReservationsModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
