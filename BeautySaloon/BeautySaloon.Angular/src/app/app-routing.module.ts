import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
const routes: Routes = [

  {path: 'user', loadChildren: () => import('./views/user/user.module').then(m => m.UserModule)},
  { path: 'welcome', loadChildren: () => import('./views/welcome/welcome.module').then(m => m.WelcomeModule) },
  { path: 'auth-callback', loadChildren: () => import('./views/auth-callback/auth-callback.module').then(m => m.AuthCallbackModule) },

  { path: 'login', loadChildren: () => import('./views/login/login.module').then(m => m.LoginModule) },
  {path: "**", redirectTo: 'welcome'},

];

// @ts-ignore
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
