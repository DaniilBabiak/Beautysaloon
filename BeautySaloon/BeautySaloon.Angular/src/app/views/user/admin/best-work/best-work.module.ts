import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BestWorkComponent } from './best-work.component';
import { FormsModule } from "@angular/forms";
import { Ng2SearchPipeModule } from "ng2-search-filter";
import { BestWorkWithImageFilter } from 'src/app/shared/helpers/best-work-with-image-filter';
import { BestWorkRoutingModule } from './best-work-routing.module';

@NgModule({
  declarations: [
    BestWorkComponent,
    BestWorkWithImageFilter
  ],
  imports: [
    CommonModule,
    BestWorkRoutingModule,
    FormsModule,
    Ng2SearchPipeModule,
  ]
})
export class BestWorkModule { }
