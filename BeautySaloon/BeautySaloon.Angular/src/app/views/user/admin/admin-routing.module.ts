import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  { path: 'add-category', loadChildren: () => import('./add-category/add-category.module').then(m => m.AddCategoryModule) },
  { path: 'add-best-works', loadChildren: () => import('./add-best-works/add-best-works.module').then(m => m.AddBestWorksModule) },
  { path: 'add-service', loadChildren: () => import('./add-service/add-service.module').then(m => m.AddServiceModule) },
  { path: 'master', loadChildren: () => import('./master/master.module').then(m => m.MasterModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
