import { HttpClient, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ConfigService } from '../configs/config.service';
import { AuthService } from './auth.service';

export abstract class BaseService {

    constructor(protected http: HttpClient, protected configService: ConfigService, protected authService: AuthService) { }

    protected handleError(error: any) {

        var applicationError = error.headers.get('Application-Error');

        // either application-error in header or model error in body
        if (applicationError) {
            return throwError(applicationError);
        }

        var modelStateErrors: string = '';

        // for now just concatenate the error descriptions, alternative we could simply pass the entire error response upstream
        for (var key in error.error) {
            if (error.error[key]) modelStateErrors += error.error[key].description + '\n';
        }

        modelStateErrors = modelStateErrors || '';

        return throwError(modelStateErrors || 'Server error');
    }

    protected get token(): string {
        return this.authService.authorizationHeaderValue;
    }
}