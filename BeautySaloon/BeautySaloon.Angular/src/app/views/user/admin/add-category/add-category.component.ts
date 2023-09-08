import { Component, OnInit } from '@angular/core';
import { CategoryService } from "../../../../shared/services/category.service";
import { ImageService } from 'src/app/shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';
import { ServiceService } from 'src/app/shared/services/service.service';
import { CategoryModel } from 'src/app/shared/models/category/category-model';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { CategoryWithImage } from 'src/app/shared/models/category/category-with-image';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {
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
  selectedFile: File | null = null;
  isServiceHovered: boolean = false; // Переменная для отслеживания наведения на область категории


  constructor(private categoryService: CategoryService,
    private imageService: ImageService,
    private authService: AuthService,
    private service: ServiceService) {

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
    this.categoryService.getCategories().subscribe(result => {
      this.serviceCategories = result.map(categoryWithoutImage => {
        return {
          model: categoryWithoutImage,
          image: ''
        } as CategoryWithImage;
      });
      this.loadImages();
    });
  }

  loadServicesWithoutCategory() {
    this.service.getServices().subscribe(result => {
      console.log(result);
      this.servicesWithoutCategory = result.filter(s => s.categoryId == null);
      console.log(this.servicesWithoutCategory);
    })
  }

  loadImages() {
    this.serviceCategories.forEach(element => {
      if (element.model.imageBucket && element.model.imageFileName) {
        this.imageService.getImage(element.model.imageBucket, element.model.imageFileName).then(image => {
          element.image = image;
        });
      }
    });
  }

  // createCategory() {
  //   this.categoryService.createCategory(this.newCategory).subscribe(result => {
  //     this.loadCategories(); // Обновляем список категорий после создания
  //     this.newCategory = {
  //       id: null,
  //       name: null,
  //       description: null,
  //       imageBucket: null,
  //       imageFileName: null,
  //       services: null,
  //       image: null
  //     };
  //     this.showAddCategoryForm = false;
  //   });
  // }

  // onFileSelected(event: any) {
  //   this.selectedFile = event.target.files[0] as File;
  //   this.imageService.uploadImage(this.selectedFile, "category").subscribe(result => {
  //     this.newCategory.imageBucket = result.bucketName;
  //     this.newCategory.imageFileName = result.fileName;
  //     if (this.newCategory.imageBucket && this.newCategory.imageFileName) {
  //       this.imageService.getImage(this.newCategory.imageBucket, this.newCategory.imageFileName).then(data => {
  //         this.newCategory.image = data;
  //       })
  //     }

  //   })
  // }

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
    if (this.serviceCategories) {
      const categoryIndex = this.serviceCategories.indexOf(category);
      // Перемещение сервиса в выбранную категорию
      if (this.serviceCategories[categoryIndex].services) {
        var alreadyInThisCategory = this.serviceCategories[categoryIndex].services?.findIndex(service => service.id === serviceData.id) != -1;
        if (!alreadyInThisCategory) {
          this.serviceCategories[categoryIndex].services?.push(serviceData);
        }
      }

      // Удаление сервиса из списка без категории
      const index = this.servicesWithoutCategory?.findIndex(service => service.id === serviceData.id) as number;
      if (index !== -1) {
        this.servicesWithoutCategory?.splice(index, 1);
      }
      this.categoryService.updateCategory(this.serviceCategories[categoryIndex].model).subscribe(() => {
        this.loadCategories();
        this.loadServicesWithoutCategory();
      });
    }
  }

  onDropRemoveService(event: any) {
    event.preventDefault();

    const serviceData = JSON.parse(event.dataTransfer.getData('text')) as ServiceModel; // Получение данных сервиса

    // Добавление сервиса в список сервисов без категории
    if (this.servicesWithoutCategory) {
      var alreadyInThisCategory = this.servicesWithoutCategory?.findIndex(service => service.id === serviceData.id) != -1;
      if (!alreadyInThisCategory) {
        this.servicesWithoutCategory.push(serviceData);
      }
    }

    // Поиск и удаление сервиса из категорий
    if (this.serviceCategories) {
      const categoryIndex = this.serviceCategories.findIndex(category => category.model.id == serviceData.categoryId);
      if (categoryIndex !== -1) {
        this.serviceCategories[categoryIndex].services?.splice(categoryIndex, 1);
        serviceData.categoryId = 0;
        this.service.updateService(serviceData).subscribe(() => {
          this.loadCategories();
        });
      }
    }

  }
}
