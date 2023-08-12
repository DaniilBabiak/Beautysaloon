import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ServiceCategory } from '../models/service-category';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};
@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
    httpOptions.headers.append('Authorization', this.auth.authorizationHeaderValue);
  }

  getCategories(): Observable<ServiceCategory[]> {
    var url = this.config.resourceApiURI;

    var options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
    console.log(this.auth.authorizationHeaderValue);
    options.headers.set('Authorization', this.auth.authorizationHeaderValue);

    return this.http.get<ServiceCategory[]>(url + '/api/ServiceCategory', options);

  }

}
