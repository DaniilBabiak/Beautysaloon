import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddBestWorksComponent } from './add-best-works.component';

const routes: Routes = [{ path: '', component: AddBestWorksComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AddBestWorksRoutingModule { }
