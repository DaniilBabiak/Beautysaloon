import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorksRoutingModule } from './works-routing.module';
import { WorksComponent } from './works.component';
import { SelectedWorkComponent } from './selected-work/selected-work.component';



@NgModule({
  declarations: [
    WorksComponent,
    SelectedWorkComponent
  ],
  imports: [
    CommonModule,
    WorksRoutingModule,

  ]
})
export class WorksModule { }
