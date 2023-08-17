import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MasterRoutingModule } from './master-routing.module';
import { MasterComponent } from './master.component';
import { MasterDetailsComponent } from './details/master-details.component';


@NgModule({
  declarations: [
    MasterComponent,
    MasterDetailsComponent
  ],
  imports: [
    CommonModule,
    MasterRoutingModule
  ]
})
export class MasterModule { }
