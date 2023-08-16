import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddCategoryRoutingModule } from './add-category-routing.module';
import { AddCategoryComponent } from './add-category.component';
import {FormsModule} from "@angular/forms";
import {SharedModule} from "../../../../shared/shared.module";


@NgModule({
  declarations: [
    AddCategoryComponent
  ],
    imports: [
        CommonModule,
        AddCategoryRoutingModule,
        FormsModule,
        SharedModule,
    ]
})
export class AddCategoryModule { }
