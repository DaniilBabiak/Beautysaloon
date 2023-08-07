import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ScrollAnimatorDirective} from "./directives/scroll-reposition.directive";

@NgModule({
  declarations: [
    ScrollAnimatorDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ScrollAnimatorDirective
  ]
})
export class SharedModule { }
