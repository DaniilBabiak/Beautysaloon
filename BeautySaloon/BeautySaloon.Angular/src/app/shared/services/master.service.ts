import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Service } from '../models/service';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Master } from '../models/master';
import { Schedule } from '../models/schedule';

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

  getSchedule(masterId: number): Observable<Schedule> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule/${masterId}`
    var options = this.getOptions();

    return this.http.get<Schedule>(url, options);
  }

  createSchedule(masterId: number, schedule: Schedule): Observable<Schedule> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule/${masterId}`
    var options = this.getOptions();

    return this.http.post<Schedule>(url, schedule, options);
  }

  updateSchedule(schedule: Schedule): Observable<Schedule> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule`
    var options = this.getOptions();

    return this.http.put<Schedule>(url, schedule, options);
  }

  createMaster(master: Master): Observable<Master> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    var body = {
      name: master.name,
      serviceIds: master.services?.map(service => service.id)
    };

    return this.http.post<Master>(`${url}/api/admin/Master`, body, options);
  }

  updateMaster(master: Master): Observable<Master> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    var body = {
      id: master.id,
      name: master.name,
      serviceIds: master.services?.map(service => service.id)
    };

    return this.http.put<Master>(`${url}/api/admin/Master`, body, options);
  }

  deleteMaster(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/admin/Master/${id}`, options);
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
