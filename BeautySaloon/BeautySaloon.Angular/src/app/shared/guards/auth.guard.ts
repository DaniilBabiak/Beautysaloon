import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from "../services/auth.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }
  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Promise<boolean> {
    await this.authService.loadUser();

    if (this.authService.isAuthenticated) {
      return true;
    }
    else{
      var url = state.url;
      this.authService.redirectUrl = url;
      this.authService.login();
      return false;
    }
  }

}
