import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { CategoryModel } from '../models/category/category-model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  constructor(private auth: AuthService, private config: ConfigService, private http: HttpClient) {
  }

  getCategories(): Observable<CategoryModel[]> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.get<CategoryModel[]>(url + '/api/ServiceCategory', options);

  }

  createCategory(serviceCategory: CategoryModel): Observable<CategoryModel> {
    var url = this.config.resourceApiURI;

    var options = this.getOptions();

    return this.http.post<CategoryModel>(`${url}/api/admin/ServiceCategory`, serviceCategory, options);
  }

  updateCategory(serviceCategory: CategoryModel): Observable<CategoryModel>{
    var url = this.config.resourceApiURI;
    
    var options = this.getOptions();

    return this.http.put<CategoryModel>(`${url}/api/admin/ServiceCategory`, serviceCategory, options)
  }

  deleteCategory(id: number): Observable<any> {
    var url = this.config.resourceApiURI;
    var options = this.getOptions();
    return this.http.delete(`${url}/api/admin/ServiceCategory/${id}`, options);
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
