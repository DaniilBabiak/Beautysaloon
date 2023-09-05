import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetAllReservationsResponse } from '../models/resrvation/get-all-reservations-response';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { ReservationModel } from '../models/resrvation/reservation-model';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getAllReservations(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string = 'DateTime'
  ): Observable<GetAllReservationsResponse> {

    const url = `${this.config.resourceApiURI}/api/admin/reservation/getAll`;
    var options = this.getOptions();

    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('sortBy', sortBy);

    return this.http.get<GetAllReservationsResponse>(url, { params, ...options });

  }

  getAvailableReservations(serviceId: number): Observable<ReservationModel[]> {
    var url = this.config.resourceApiURI;
    url = url + `/api/reservation/getAvailable/${serviceId}`;
    var options = this.getOptions();

    return this.http.get<ReservationModel[]>(url, options);
  }

  createReservation(reservationRequest: ReservationModel): Observable<any> {
    var url = this.config.resourceApiURI;
    url = url + `/api/reservation/`;
    var options = this.getOptions();

    return this.http.post(url, reservationRequest, options);
  }

  private getOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.auth.authorizationHeaderValue
      })
    };
  }
  deleteReservation(reservationId: number): Observable<any> {
    const url = `${this.config.resourceApiURI}/api/reservation/${reservationId}`;
    const options = this.getOptions();

    return this.http.delete(url, options);
  }
}
