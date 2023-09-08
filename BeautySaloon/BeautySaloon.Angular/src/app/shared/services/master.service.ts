import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { MasterDetailedModel } from '../models/master/master-detailed-model';
import { MasterModel } from '../models/master/master-model';
import { ScheduleModel } from '../models/schedule/schedule-model';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getAllMasters(): Observable<MasterModel[]> {
    var url = this.config.resourceApiURI;
    url = url + '/api/master'
    var options = this.getOptions();

    return this.http.get<MasterModel[]>(url, options);

  }

  getMaster(id: number): Observable<MasterDetailedModel> {
    var url = this.config.resourceApiURI;
    url = url + `/api/master/${id}`
    var options = this.getOptions();

    return this.http.get<MasterDetailedModel>(url, options);
  }

  getScheduleByMasterId(masterId: number): Observable<ScheduleModel> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule?masterId=${masterId}`
    var options = this.getOptions();

    return this.http.get<ScheduleModel>(url, options);
  }

  getSchedule(scheduleId: number): Observable<ScheduleModel> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule?scheduleId=${scheduleId}`
    var options = this.getOptions();

    return this.http.get<ScheduleModel>(url, options);
  }

  createSchedule(schedule: ScheduleModel): Observable<ScheduleModel> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule/`
    var options = this.getOptions();

    return this.http.post<ScheduleModel>(url, schedule, options);
  }

  updateSchedule(schedule: ScheduleModel): Observable<ScheduleModel> {
    var url = this.config.resourceApiURI;
    url = url + `/api/admin/master/schedule`
    var options = this.getOptions();

    return this.http.put<ScheduleModel>(url, schedule, options);
  }

  createMaster(master: MasterDetailedModel): Observable<MasterDetailedModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<MasterDetailedModel>(`${url}/api/admin/Master`, master, options);
  }

  updateMaster(master: MasterDetailedModel): Observable<MasterDetailedModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.put<MasterDetailedModel>(`${url}/api/admin/Master`, master, options);
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
