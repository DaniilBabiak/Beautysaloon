import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from './config.service';
import { Observable, of } from 'rxjs';
import { Profile } from '../models/profile/profile';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) { }

  getProfile(): Observable<Profile | null> {
    if (!this.auth.isAuthenticated) {
      return of(null)
    }
    var url = this.config.resourceApiURI;
    url = url + `/api/customer/profile`
    var options = this.getOptions();

    return this.http.get<Profile>(url, options);
  }

  updateSchedule(profile: Profile): Observable<Profile> {
    if (!this.auth.isAuthenticated) {
      throw "Error";
    }
    var url = this.config.resourceApiURI;
    url = url + `/api/customer/profile`
    var options = this.getOptions();

    return this.http.put<Profile>(url, profile, options);
  }

  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }
}
