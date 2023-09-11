import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule} from "@angular/forms";
import {SharedModule} from "../../../../shared/shared.module";
import { CategoryComponent } from './category.component';
import { CategoryRoutingModule } from './category-routing.module';
import { CategoryDetailsComponent } from './category-details/category-details.component';


@NgModule({
  declarations: [
    CategoryComponent,
    CategoryDetailsComponent
  ],
    imports: [
        CommonModule,
        CategoryRoutingModule,
        FormsModule,
        SharedModule,
    ]
})
export class CategoryModule { }
