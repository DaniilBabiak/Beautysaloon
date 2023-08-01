import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Observable, catchError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from '../configs/config.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TestRestService extends BaseService {

  constructor(
    protected override http: HttpClient,
    protected override configService: ConfigService,
    protected override authService: AuthService) {
    super(http, configService, authService)
  }

  getWeatherForecast(): Observable<string[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.token
      })
    };
    return this.http.get<string[]>(this.configService.resourceApiURI + '/WeatherForecast', httpOptions).pipe(catchError(this.handleError));
  }
}
