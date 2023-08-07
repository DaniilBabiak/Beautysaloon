import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { FrontPageComponent } from './front-page/front-page.component';
import { HttpClientModule } from '@angular/common/http';
import { ConfigService } from './configs/config.service';
import { FormsModule } from '@angular/forms';

import { AuthCallbackComponent } from './auth/auth-callback/auth-callback.component';
import { ProfileComponent } from './profileComponents/profile/profile.component';
import { ChangePhoneNumberFormComponent } from './profileComponents/change-phone-number-form/change-phone-number-form.component';
import { TestRestComponent } from './test-rest/test-rest.component';



@NgModule({
  declarations: [
    AppComponent,
   
    FrontPageComponent,
   
    AuthCallbackComponent,
         ProfileComponent,
         ChangePhoneNumberFormComponent,
         TestRestComponent,

  

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
  
  ],
  providers: [ConfigService],
  bootstrap: [AppComponent]
})
export class AppModule { }
