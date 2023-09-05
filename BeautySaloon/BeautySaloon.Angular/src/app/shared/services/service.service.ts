import { Injectable, Output } from '@angular/core';
import { AuthService } from "./auth.service";
import { ConfigService } from "./config.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { ServiceModel } from '../models/service/service-model';
import { ServiceDetailedModel } from '../models/service/service-detailed-model';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getServices(categoryId: number | null = null): Observable<ServiceModel[]> {
    var url = this.config.resourceApiURI;

    if (categoryId) {
      url = url + '/api/Service?categoryId=' + categoryId;
    }
    else {
      url = url + '/api/Service'
    }

    var options = this.getOptions();

    return this.http.get<ServiceModel[]>(url, options);
  }

  getService(serviceId: number):Promise<ServiceDetailedModel | undefined>{
    const url = `${this.config.resourceApiURI}/${serviceId}`;

    var options = this.getOptions();

    return this.http.get<ServiceDetailedModel>(url, options).toPromise();
  }

  createService(service: ServiceModel): Observable<ServiceModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<ServiceModel>(`${url}/api/admin/Service`, service, options);
  }

  updateService(service: ServiceModel): Observable<ServiceModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.put<ServiceModel>(`${url}/api/admin/Service`, service, options);
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
