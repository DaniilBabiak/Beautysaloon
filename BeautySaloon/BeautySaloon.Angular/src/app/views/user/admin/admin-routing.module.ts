import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  { path: 'add-service', loadChildren: () => import('./add-service/add-service.module').then(m => m.AddServiceModule) },
  { path: 'add-best-works', loadChildren: () => import('./add-best-works/add-best-works.module').then(m => m.AddBestWorksModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
