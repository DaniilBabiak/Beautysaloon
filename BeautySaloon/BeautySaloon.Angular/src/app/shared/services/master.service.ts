import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Service } from '../models/service';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Master } from '../models/master';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getAllMasters(): Observable<Master[]> {
    var url = this.config.resourceApiURI;
    url = url + '/api/customer/master'
    var options = this.getOptions();

    return this.http.get<Master[]>(url, options);

  }

  getMaster(id: number): Observable<Master> {
    var url = this.config.resourceApiURI;
    url = url + `/api/customer/master/${id}`
    var options = this.getOptions();

    return this.http.get<Master>(url, options);
  }

  createService(service: Service): Observable<Service> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<Service>(`${url}/api/admin/Service`, service, options);
  }

  updateService(service: Service): Observable<Service> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.put<Service>(`${url}/api/admin/Service`, service, options);
  }

  deleteService(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/admin/Service/${id}`, options);
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
