import { Injectable } from '@angular/core';
import { OidcClientSettings, UserManagerSettings } from 'oidc-client';
@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  constructor() { }

  get resourceApiURI() {
    return 'http://localhost:5020';
  }

  get imageApiURI(){
    return 'http://localhost:5040';
  }
get healthCheckURI(){
    return 'http://localhost:5030/';
}
  getClientSettings(): UserManagerSettings {
    return {
      authority: 'https://localhost:5001',
      client_id: 'angular_spa',
      client_secret: 'secret',
      redirect_uri: 'http://localhost:4200/auth-callback',
      post_logout_redirect_uri: 'http://localhost:4200/user/main',
      response_type: "id_token token",
      scope: "openid profile email api.read api.edit roles image.read image.edit",
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
    };
  }
}
