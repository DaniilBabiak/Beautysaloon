import { Injectable } from '@angular/core';
import { UserManagerSettings } from 'oidc-client';

@Injectable()
export class ConfigService {

    constructor() { }

    get resourceApiURI() {
        return 'http://localhost:5002';
    }

    getClientSettings(): UserManagerSettings {
        return {
            authority: 'https://localhost:5001',
            client_id: 'angular_spa',
            redirect_uri: 'http://localhost:4200/auth-callback',
            post_logout_redirect_uri: 'http://localhost:4200/user/main',
            response_type: "id_token token",
            scope: "openid profile email api.read api.edit role",
            filterProtocolClaims: true,
            loadUserInfo: true,
            automaticSilentRenew: true,
            silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
        };
    }
}