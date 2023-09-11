import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserManager, User, OidcClient } from 'oidc-client';
import { BehaviorSubject, catchError, map, throwError } from 'rxjs';
import { ConfigService } from "./config.service";

const REDIRECT_URL_KEY = 'redirectUrl';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private _isAdminNavStatusSource = new BehaviorSubject<boolean>(false);

  isAdminNavStatus$ = this._isAdminNavStatusSource.asObservable();

  private manager: UserManager | null = null;
  private user: User | null = null;
  private tokenResponse: any;

  constructor(private http: HttpClient, private configService: ConfigService) {
    var userManageConfig = configService.getClientSettings();
    this.manager = new UserManager(userManageConfig);
  }

  login() {
    if (this.manager) {
      return this.manager.signinRedirect();
    }

    return null;
  }

  loadUser() {
    return this.manager?.getUser().then(async user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated);
      this._isAdminNavStatusSource.next(this.isAdmin);

      var settings = this.configService.getClientSettings();
      var tokensUrl = `${settings.authority}/connect/token`;
      const formData = new URLSearchParams();
      formData.set('client_id', settings.client_id ?? '');
      formData.set('client_secret', settings.client_secret ?? '');
      formData.set('grant_type', 'client_credentials');

      const headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      this.tokenResponse = await this.http.post<any>(tokensUrl, formData.toString(), { headers }).toPromise();
    });
  }

  async completeAuthentication() {
    if (this.manager) {
      this.user = await this.manager.signinRedirectCallback();
      this._authNavStatusSource.next(this.isAuthenticated);
      this._isAdminNavStatusSource.next(this.isAdmin);
    }
  }

  get isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  get isAdmin(): boolean {
    var result = this.user != null && !this.user.expired && this.user.profile["role"] == 'admin';
    return result;
  }

  get authorizationHeaderValue(): string {
    if (this.user) {
      return `${this.user.token_type} ${this.user.access_token}`;
    }

    if (this.tokenResponse) {
      return `${this.tokenResponse.token_type} ${this.tokenResponse.access_token}`;
    }

    return '';
  }

  get name(): string {
    return this.user?.profile?.name ?? '';
  }

  get redirectUrl(): string | null {
    return sessionStorage.getItem(REDIRECT_URL_KEY);
  }

  set redirectUrl(url: string | null) {
    if (url) {
      sessionStorage.setItem(REDIRECT_URL_KEY, url);
    } else {
      sessionStorage.removeItem(REDIRECT_URL_KEY);
    }
  }
  get phoneNumber(): string {
    return this.user?.profile?.phone_number ?? '';
  }

  async signout() {
    if (this.manager) {
      await this.manager.signoutRedirect();

    }
  }

  handleError(error: any) {

    var applicationError = error.headers.get('Application-Error');

    // either application-error in header or model error in body
    if (applicationError) {
      return throwError(applicationError);
    }

    var modelStateErrors: string | null = '';

    // for now just concatenate the error descriptions, alternative we could simply pass the entire error response upstream
    for (var key in error.error) {
      if (error.error[key]) modelStateErrors += error.error[key].description + '\n';
    }

    modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
    return throwError(modelStateErrors || 'Server error');
  }
}
