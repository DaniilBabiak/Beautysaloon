import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterComponent } from './master.component';
import { MasterDetailsComponent } from './details/master-details.component';

const routes: Routes =
[
  { path: '', component: MasterComponent },
  { path: 'details/:id', component: MasterDetailsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MasterRoutingModule { }
