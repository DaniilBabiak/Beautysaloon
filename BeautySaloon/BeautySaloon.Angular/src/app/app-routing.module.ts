import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';



const routes: Routes = [

  {path: 'user', loadChildren: () => import('./views/user/user.module').then(m => m.UserModule)},
  { path: 'welcome', loadChildren: () => import('./views/welcome/welcome.module').then(m => m.WelcomeModule) },
  {path: "**", redirectTo: 'welcome'},

  // { path: '', redirectTo: 'user', pathMatch: 'full' },
  // { path: 'welcome', component: FrontPageComponent },
  //
  //
  //
  // { path: 'admin', loadChildren: () => import('./modules/admin/admin.module').then((m) => m.AdminModule) },
  // { path: 'user', loadChildren: () => import('./modules/user/user.module').then((m) => m.UserModule) },
  // { path: 'profile', component: ProfileComponent },
  // { path: 'auth-callback', component: AuthCallbackComponent },
  // { path: 'test', component: TestRestComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
