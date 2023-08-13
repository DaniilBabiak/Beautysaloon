import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }
  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Promise<boolean> {
    await this.authService.loadUser();
    
    if (this.authService.isAdmin) {
      return true;
    }
    else{
      return false;
    }
  }
}




