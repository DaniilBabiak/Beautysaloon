import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BestWorkComponent } from './best-work.component';

const routes: Routes = [{ path: '', component: BestWorkComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BestWorkRoutingModule { }
