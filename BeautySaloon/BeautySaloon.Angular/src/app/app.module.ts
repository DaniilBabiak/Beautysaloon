import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule, NgbTimeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from "@angular/common/http";
import { ConfigService } from "./shared/services/config.service";
import { NgbTimeStringAdapter } from './shared/helpers/ngb-time-string-adapter';
@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FontAwesomeModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [ConfigService, { provide: NgbTimeAdapter, useClass: NgbTimeStringAdapter }],
  bootstrap: [AppComponent]
})
export class AppModule { }
