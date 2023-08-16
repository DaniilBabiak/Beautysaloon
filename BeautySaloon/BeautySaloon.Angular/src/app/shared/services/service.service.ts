import { Injectable } from '@angular/core';
import { AuthService } from "./auth.service";
import { ConfigService } from "./config.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { Service } from "../models/service";

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getServices(categoryId: number | null = null): Observable<Service[]> {
    var url = this.config.resourceApiURI;

    if (categoryId) {
      url = url + '/api/Service?categoryId=' + categoryId;
    }
    else {
      url = url + '/api/Service'
    }

    var options = this.getOptions();

    return this.http.get<Service[]>(url, options);

  }

  createService(service: Service): Observable<Service> {


    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<Service>(`${url}/api/admin/Service`, service, options);
  }

  updateService(service: Service):Observable<Service>{
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
