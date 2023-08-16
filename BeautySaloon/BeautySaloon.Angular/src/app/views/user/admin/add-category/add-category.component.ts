import { Component, OnInit } from '@angular/core';
import { CategoryService } from "../../../../shared/services/category.service";
import { ServiceCategory } from "../../../../shared/models/service-category";
import { ImageService } from 'src/app/shared/services/image.service';
import { AuthService } from '../../../../shared/services/auth.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {
  serviceCategories: ServiceCategory[] | null = null;
  newCategory: ServiceCategory = {
    id: null,
    name: null,
    description: null,
    imageBucket: null,
    imageFileName: null,
    services: null,
    image: null
  };
  showAddCategoryForm = false;
  selectedFile: File | null = null;

  constructor(private categoryService: CategoryService, private imageService: ImageService, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.loadCategories();
    })
  }

  deleteCategory(id: number | null) {
    if (id) {
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
      if (element.imageBucket && element.imageFileName) {
        this.imageService.getImage(element.imageBucket, element.imageFileName).then(data => {
          element.image = data;

        });
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
        imageFileName: null,
        services: null,
        image: null
      };
      this.showAddCategoryForm = false;
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
    this.imageService.uploadImage(this.selectedFile, "category").subscribe(result => {
      this.newCategory.imageBucket = result.bucketName;
      this.newCategory.imageFileName = result.fileName;
      if (this.newCategory.imageBucket && this.newCategory.imageFileName) {
        this.imageService.getImage(this.newCategory.imageBucket, this.newCategory.imageFileName).then(data => {
          this.newCategory.image = data;
        })
      }

    })
  }
}
