import { Component, EventEmitter, OnInit } from '@angular/core';
import { CategoryService } from '../../../../shared/services/category.service';
import { ImageService } from '../../../../shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { ServiceService } from '../../../../shared/services/service.service';
import { NgbTimeStringAdapter } from '../../../../shared/helpers/ngb-time-string-adapter'
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { ServiceDetailedModel } from 'src/app/shared/models/service/service-detailed-model';

@Component({
  selector: 'app-add-service',
  templateUrl: './add-service.component.html',
  styleUrls: ['./add-service.component.css'],
})
export class AddServiceComponent implements OnInit {
  isFieldsValid: any = false;
  isTimeValid = false;
  services: ServiceDetailedModel[] | null = null;
  categories: CategoryModel[] | null = null;
  newService: ServiceDetailedModel = {
    id: 0,
    name: "",
    duration: "00:00:00",
    price: 0,
    categoryId: 0,
    masterIds: [],
    reservationIds: []
  };
  showAddServiceForm = false;

  constructor(
    private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private service: ServiceService,
    private NgbTimeStringAdapter: NgbTimeStringAdapter
  ) { }

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
      this.categories = result;
    });
  }

  async loadImageAsync(category: CategoryModel) {
    return this.imageService.getImage(category.imageBucket, category.imageFileName);
  }

  selectCategory(category: CategoryModel) {
    this.newService.categoryId = category.id;
  }
  // saveService() {
  //   this.service.createService(this.newService).subscribe(result => {
  //     this.newService = {
  //       id: null,
  //       name: null,
  //       duration: "00:00:00",
  //       category: null,
  //       reservations: null,
  //       price: null,
  //       categoryId: null,
  //       masters: null
  //     }
  //   })
  // }

  checkFieldsValidity() {
    this.isFieldsValid =
      this.newService.name && this.newService.price && this.isTimeValid;
  }
  checkTimeValidity() {
    this.isTimeValid = !!this.newService.duration;
    this.checkFieldsValidity(); // Проверка всех полей после изменения времени
  }
  handleMouseWheel(event: WheelEvent) {
    const container = document.querySelector('.service-wrapper') as HTMLElement;
    if (container) {
      container.scrollLeft += event.deltaY;
      event.preventDefault(); // Остановим стандартную прокрутку страницы
    }
  }
}
