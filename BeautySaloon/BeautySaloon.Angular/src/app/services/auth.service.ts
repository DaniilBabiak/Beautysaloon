import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserManager, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { getClientSettings } from '../configs/auth.config';
import { ConfigService } from '../configs/uri.config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private manager = new UserManager(getClientSettings());
  private user: User | null = null;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.manager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  login() {
    return this.manager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());
  }

  get authorizationHeaderValue(): string {
    return this.user != null ? `${this.user.token_type} ${this.user.access_token}` : '';
  }

  get name(): string | undefined {
    return this.user != null ? this.user.profile.name : '';
  }

  async signout() {
    await this.manager.signoutRedirect();
  }
}
