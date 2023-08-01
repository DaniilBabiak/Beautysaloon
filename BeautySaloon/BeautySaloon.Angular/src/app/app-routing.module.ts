import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FrontPageComponent } from './front-page/front-page.component';

import { AuthCallbackComponent } from './auth/auth-callback/auth-callback.component';
import { ProfileComponent } from './profileComponents/profile/profile.component';


const routes: Routes = [
  { path: '', redirectTo: 'user', pathMatch: 'full' },
  { path: 'welcome', component: FrontPageComponent },

  { path: '', redirectTo: 'user', pathMatch: 'full' },
  { path: 'welcome', component: FrontPageComponent },
  //{path:'**', redirectTo:'admin', pathMatch:'full'},
  { path: 'admin', loadChildren: () => import('./modules/admin/admin.module').then((m) => m.AdminModule) },
  { path: 'user', loadChildren: () => import('./modules/user/user.module').then((m) => m.UserModule) },
  { path: 'profile', component: ProfileComponent },
  { path: 'auth-callback', component: AuthCallbackComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
