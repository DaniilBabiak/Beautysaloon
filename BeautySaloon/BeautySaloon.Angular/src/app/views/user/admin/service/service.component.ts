import { Component, EventEmitter, OnInit } from '@angular/core';
import { CategoryService } from '../../../../shared/services/category.service';
import { ImageService } from '../../../../shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { ServiceService } from '../../../../shared/services/service.service';
import { NgbTimeStringAdapter } from '../../../../shared/helpers/ngb-time-string-adapter'
import { NgbModal, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { ServiceDetailedModel } from 'src/app/shared/models/service/service-detailed-model';
import { ServiceDetailsComponent } from './service-details/service-details.component';
import { CategoryWithImage } from 'src/app/shared/models/category/category-with-image';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css'],
})
export class ServiceComponent implements OnInit {
  isFieldsValid: any = false;
  isTimeValid = false;
  categories: CategoryWithImage[] = [];

  constructor(
    private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private modalService: NgbModal,
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
    this.categoryService.getCategories().subscribe(categories => {
      const categoryPromises = categories.map(element => {
        const categoryWithImage: CategoryWithImage = {
          model: element,
          image: '',
          services: []
        };

        const imagePromise = this.imageService.getImage(element.imageBucket, element.imageFileName)
          .then(image => {
            return image;
          })
          .catch(error => {
            return 'assets/images/image-not-found.jpg';
          });
        const servicesPromise = this.service.getServices(element.id).toPromise();

        return Promise.all([imagePromise, servicesPromise]).then(([image, services]) => {
          categoryWithImage.image = image;
          categoryWithImage.services = services as ServiceModel[];
          return categoryWithImage;
        });
      });

      Promise.all(categoryPromises).then(categoriesWithImages => {
        this.categories = categoriesWithImages;
      });
    });
  }

  openServiceDetails(service: ServiceModel) {
    const modalRef = this.modalService.open(ServiceDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.serviceId = service.id;
    modalRef.componentInstance.initModal();

    modalRef.closed.subscribe(result => {
      this.loadCategories();
    })
  }

  // checkFieldsValidity() {
  //   this.isFieldsValid =
  //     this.newService.name && this.newService.price && this.isTimeValid;
  // }
  // checkTimeValidity() {
  //   this.isTimeValid = !!this.newService.duration;
  //   this.checkFieldsValidity(); // Проверка всех полей после изменения времени
  // }
  // handleMouseWheel(event: WheelEvent) {
  //   const container = document.querySelector('.service-wrapper') as HTMLElement;
  //   if (container) {
  //     container.scrollLeft += event.deltaY;
  //     event.preventDefault(); // Остановим стандартную прокрутку страницы
  //   }
  // }
}
