import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddBestWorksRoutingModule } from './add-best-works-routing.module';
import { AddBestWorksComponent } from './add-best-works.component';


@NgModule({
  declarations: [
    AddBestWorksComponent
  ],
  imports: [
    CommonModule,
    AddBestWorksRoutingModule
  ]
})
export class AddBestWorksModule { }
