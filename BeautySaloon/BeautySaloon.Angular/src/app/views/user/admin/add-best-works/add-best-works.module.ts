import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddBestWorksRoutingModule } from './add-best-works-routing.module';
import { AddBestWorksComponent } from './add-best-works.component';
import { FormsModule } from "@angular/forms";
import { Ng2SearchPipeModule } from "ng2-search-filter";
import { BestWorkWithImageFilter } from 'src/app/shared/helpers/best-work-with-image-filter';

@NgModule({
  declarations: [
    AddBestWorksComponent,
    BestWorkWithImageFilter
  ],
  imports: [
    CommonModule,
    AddBestWorksRoutingModule,
    FormsModule,
    Ng2SearchPipeModule,
  ]
})
export class AddBestWorksModule { }
