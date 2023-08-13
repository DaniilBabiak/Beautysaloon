import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ServiceCategory } from '../models/service-category';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getCategories(): Observable<ServiceCategory[]> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.get<ServiceCategory[]>(url + '/api/ServiceCategory', options);

  }

  createCategory(serviceCategory: ServiceCategory): Observable<ServiceCategory> {

    const categoryWithoutImage: ServiceCategory = {
      id: serviceCategory.id,
      name: serviceCategory.name,
      description: serviceCategory.description,
      imageBucket: serviceCategory.imageBucket,
      imageUrl: serviceCategory.imageUrl,
      services: serviceCategory.services,
      image: null // Указываем, что image равен null
    };

    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<ServiceCategory>(`${url}/api/ServiceCategory`, categoryWithoutImage, options);
  }
deleteCategory(id:number):Observable<any>{
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/ServiceCategory/${id}`, options);
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
