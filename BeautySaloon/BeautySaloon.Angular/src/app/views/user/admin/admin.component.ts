import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../shared/services/category.service';
import { ServiceCategory } from '../../../shared/models/service-category';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{
  serviceCategories: ServiceCategory[] | null = null;

  constructor(private categoryService: CategoryService) {

  }
  ngOnInit(): void {
    this.categoryService.getCategories().subscribe(result =>{
      this.serviceCategories = result;
    })
  }
}
