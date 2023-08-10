import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserManager, User } from 'oidc-client';
import { BehaviorSubject, catchError, throwError } from 'rxjs';
import { ConfigService} from "./config.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private manager: UserManager | null = null;
  private user: User | null = null;

  constructor(private http: HttpClient, private configService: ConfigService) {
    var userManageConfig = configService.getClientSettings();
    this.manager = new UserManager(userManageConfig);
    this.manager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  login() {
    if (this.manager) {
      return this.manager.signinRedirect();
    }

    return null;
  }

  async completeAuthentication() {
    if (this.manager) {
      this.user = await this.manager.signinRedirectCallback();
      this._authNavStatusSource.next(this.isAuthenticated());
    }
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  isAdmin(): boolean{
    var role = this.user?.profile["role"];
    return this.user?.profile["role"] == 'admin';
  }

  get authorizationHeaderValue(): string {
    if (this.user) {
      return `${this.user.token_type} ${this.user.access_token};`
    }
    return '';
  }

  get name(): string {
    return this.user?.profile?.name ?? '';
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
