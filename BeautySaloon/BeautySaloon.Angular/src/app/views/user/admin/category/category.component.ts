import { Component, OnInit } from '@angular/core';
import { CategoryService } from "../../../../shared/services/category.service";
import { ImageService } from 'src/app/shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { ServiceService } from 'src/app/shared/services/service.service';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { CategoryWithImage } from 'src/app/shared/models/category/category-with-image';
import { ServiceDetailedModel } from 'src/app/shared/models/service/service-detailed-model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryDetailsComponent } from './category-details/category-details.component';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  divDisplayStyle = 'flex';
  showServiceList() {
    this.divDisplayStyle = 'flex';
  }
  hideServiceList() {
    this.divDisplayStyle = 'none';
  }
  serviceCategories: CategoryWithImage[] = [];
  servicesWithoutCategory: ServiceModel[] | null = null;

  showAddCategoryForm = false;
  isServiceHovered: boolean = false; // Переменная для отслеживания наведения на область категории


  constructor(private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private service: ServiceService,
    private modalService: NgbModal) {

  }

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
      this.loadServicesWithoutCategory();
    })
  }

  deleteCategory(id: number | null) {
    if (id) {
      this.categoryService.deleteCategory(id).subscribe(result => {
        this.loadCategories();
        this.loadServicesWithoutCategory();
      })
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
            return 'assets/images/about-page3.png';
          });
        const servicesPromise = this.service.getServices(element.id).toPromise();

        return Promise.all([imagePromise, servicesPromise]).then(([image, services]) => {
          categoryWithImage.image = image;
          categoryWithImage.services = services as ServiceModel[];
          return categoryWithImage;
        });
      });

      Promise.all(categoryPromises).then(categoriesWithImages => {
        this.serviceCategories = categoriesWithImages;
      });
    });
  }

  loadServicesWithoutCategory() {
    this.service.getServices().subscribe(result => {
      this.servicesWithoutCategory = result.filter(s => s.categoryId == 0);
    })
  }

  openCategoryDetails(categoryId: number) {
    const modalRef = this.modalService.open(CategoryDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.categoryId = categoryId;
    modalRef.componentInstance.initModal();

    modalRef.closed.subscribe(result => {
      this.ngOnInit();
    })
  }

  // Обработчик события начала перетаскивания сервиса
  onDragStart(event: any, service: any) {
    event.dataTransfer.setData('text', JSON.stringify(service)); // Передача данных сервиса для перемещения
  }

  // Обработчик события наведения на область категории
  onDragOver(event: any) {
    event.preventDefault();
    this.isServiceHovered = true;
  }

  // Обработчик события покидания области категории
  onDragLeave() {
    this.isServiceHovered = false;
  }

  // Обработчик события сбрасывания сервиса в область категории
  onDropAddService(event: any, category: CategoryWithImage) {
    event.preventDefault();
    this.isServiceHovered = false;

    const serviceData = JSON.parse(event.dataTransfer.getData('text')); // Получение данных сервиса
    console.log(serviceData);
    category.model.serviceIds.push(serviceData.id);

    this.categoryService.updateCategory(category.model).subscribe(() => {
      this.loadCategories();
      this.loadServicesWithoutCategory();
    })
  }

  onDropRemoveService(event: any) {
    event.preventDefault();

    const serviceData = JSON.parse(event.dataTransfer.getData('text')) as ServiceModel; // Получение данных сервиса
    console.log(serviceData);

    this.service.getService(serviceData.id).then(result => {
      var serviceToUpdate = result as ServiceDetailedModel;
      serviceToUpdate.categoryId = 0;
      this.service.updateService(serviceToUpdate).subscribe(() => {
        this.loadCategories();
        this.loadServicesWithoutCategory();
      })
    })
  }
}
