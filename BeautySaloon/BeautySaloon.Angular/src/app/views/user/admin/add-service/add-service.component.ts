import {Component, OnInit} from '@angular/core';
import{CategoryService} from "../../../../shared/services/category.service";
import {ServiceCategory} from "../../../../shared/models/service-category";
import {ImageService} from 'src/app/shared/services/image.service';

@Component({
  selector: 'app-add-service',
  templateUrl: './add-service.component.html',
  styleUrls: ['./add-service.component.css']
})
export class AddServiceComponent implements OnInit {
  serviceCategories: ServiceCategory[] | null = null;
  newCategory: ServiceCategory = {
    id: null,
    name: null,
    description: null,
    imageBucket: null,
    imageUrl: null,
    services: null,
    image: null
  };
  showAddCategoryForm = false;
  selectedFile: File | null = null;

  constructor(private categoryService: CategoryService, private imageService: ImageService) {

  }

  ngOnInit(): void {
    this.loadCategories();
  }

  deleteCategory(id: number | null) {
    if (id){
      this.categoryService.deleteCategory(id).subscribe(result => {
        this.loadCategories();
      })
    }

  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(result => {
      this.serviceCategories = result;
      this.loadImages();
    });
  }

  loadImages() {
    this.serviceCategories?.forEach(element => {
      if (element.imageBucket && element.imageUrl) {
        this.imageService.getImage(element.imageBucket, element.imageUrl).subscribe(
          (data: Blob) => {
            element.image = URL.createObjectURL(data);
          })
      }
    });
  }

  createCategory() {
    this.categoryService.createCategory(this.newCategory).subscribe(result => {
      this.loadCategories(); // Обновляем список категорий после создания
      this.newCategory = {
        id: null,
        name: null,
        description: null,
        imageBucket: null,
        imageUrl: null,
        services: null,
        image: null
      };
      this.showAddCategoryForm = false;
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
    this.imageService.uploadImage(this.selectedFile, "categories").subscribe(result => {
      this.newCategory.imageBucket = result.bucketName;
      this.newCategory.imageUrl = result.fileName;
      if (this.newCategory.imageBucket && this.newCategory.imageUrl) {
        this.imageService.getImage(this.newCategory.imageBucket, this.newCategory.imageUrl).subscribe(
          (data: Blob) => {
            this.newCategory.image = URL.createObjectURL(data);
          })
      }

    })
  }
}
