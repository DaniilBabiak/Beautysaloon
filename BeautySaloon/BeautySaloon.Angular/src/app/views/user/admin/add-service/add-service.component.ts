import { Component, OnInit } from '@angular/core';
import { ServiceCategory } from '../../../../shared/models/service-category';
import { CategoryService } from '../../../../shared/services/category.service';
import { ImageService } from '../../../../shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { Service } from 'src/app/shared/models/service';
import { ServiceService } from '../../../../shared/services/service.service';

@Component({
  selector: 'app-add-service',
  templateUrl: './add-service.component.html',
  styleUrls: ['./add-service.component.css'],
})
export class AddServiceComponent implements OnInit {

  services: Service[] | null = null;
  serviceCategories: ServiceCategory[] | null = null;
  newService: Service = {
    id: null,
    name: null,
    duration: null,
    endTime: null,
    startTime: null,
    category: null,
    reservations: null,
    price: null,
    categoryId: null,
  };
  showAddServiceForm = false;

  constructor(
    private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private service: ServiceService
  ) {}

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    });
  }

  deleteCategory(id: number | null) {
    if (id) {
      this.categoryService.deleteCategory(id).subscribe((result) => {
        this.loadCategories();
      });
    }
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe((result) => {
      this.serviceCategories = result;
      this.loadImages();
    });
  }

  loadImages() {
    this.serviceCategories?.forEach((element) => {
      if (element.imageBucket && element.imageFileName) {
        this.imageService
          .getImage(element.imageBucket, element.imageFileName)
          .then((data) => {
            element.image = data;
          });
      }
    });
  }
  selectCategory(category:ServiceCategory){
    this.newService.categoryId = category.id;
    this.newService.category = category;
  }
  saveService(){
    this.service.createService(this.newService).subscribe(result => {
      this.newService  = {
        id: null,
        name: null,
        duration: null,
        endTime: null,
        startTime: null,
        category: null,
        reservations: null,
        price: null,
        categoryId: null,
      }
    })
  }
}
