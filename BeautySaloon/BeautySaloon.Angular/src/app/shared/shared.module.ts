import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ScrollAnimatorDirective} from "./directives/scroll-reposition.directive";
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import {RouterLink, RouterLinkActive} from "@angular/router";
import {FontAwesomeModule} from "@fortawesome/angular-fontawesome";
import {FormsModule} from "@angular/forms";
@NgModule({
  declarations: [
    ScrollAnimatorDirective,
    HeaderComponent,
    FooterComponent,
  ],
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    FontAwesomeModule,
    FormsModule,
  ],
  exports: [
    ScrollAnimatorDirective,
    FooterComponent,
    HeaderComponent,
  ],
  providers:[
  ]
})
export class SharedModule { }
