import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryWithImage } from 'src/app/shared/models/category/category-with-image';
import { ServiceModel } from 'src/app/shared/models/service/service-model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { ImageService } from 'src/app/shared/services/image.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { ServiceService } from 'src/app/shared/services/service.service';

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.css']
})
export class CategoryDetailsComponent {
  @Input() categoryId: number;
  isCloseAttempted: boolean = false;
  category: CategoryWithImage;
  selectedFile: File | null = null;

  isServiceHovered: boolean = false; // Переменная для отслеживания наведения на область категории
  divDisplayStyle = 'flex';
  showServiceList() {
    this.divDisplayStyle = 'flex';
  }
  hideServiceList() {
    this.divDisplayStyle = 'none';
  }
  constructor(
    public activeModal: NgbActiveModal,
    private imageService: ImageService,
    private serviceService: ServiceService,
    private auth: AuthService,
    private masterService: MasterService,
    private categoryService: CategoryService) {
    this.categoryId = 0;
    this.category = this.getEmptyCategory();
  }

  ngOnInit(): void {

  }

  initModal() {
    this.loadCategory();
  }

  loadCategory() {
    if (this.categoryId == 0) {
      this.category = this.getEmptyCategory();
    }
    else {
      this.auth.loadUser()?.then(() => {
        this.categoryService.getCategory(this.categoryId).subscribe(category => {
          const imagePromise = this.imageService.getImage(category.imageBucket, category.imageFileName)
            .then(image => {
              return image;
            })
            .catch(error => {
              return 'assets/images/about-page3.png';
            });
          const servicesPromise = this.serviceService.getServices(category.id).toPromise();

          return Promise.all([imagePromise, servicesPromise]).then(([image, services]) => {
            this.category.image = image;
            this.category.model = category;
            this.category.services = services as ServiceModel[];
          });
        })
      })
    }
  }

  getEmptyCategory(): CategoryWithImage {
    var newCategory: CategoryWithImage = {
      model: {
        id: 0,
        description: '',
        name: '',
        imageBucket: '',
        imageFileName: '',
        serviceIds: []
      },
      image: '',
      services: []
    }

    return newCategory;
  }

  closeModal() {
    this.activeModal.close();
  }

  saveCategory() {
    if (this.categoryId == 0) {
      this.categoryService.createCategory(this.category.model).subscribe(() => {
        this.closeModal();
      });
    }
    else{
      this.categoryService.updateCategory(this.category.model).subscribe(() => {
        this.closeModal();
      });
    }
  }

  deleteCategory() {
    this.auth.loadUser()?.then(() => {
      if (this.categoryId != 0) {
        this.categoryService.deleteCategory(this.categoryId).subscribe(result => {
          this.closeModal();
        })
      }
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
    this.imageService.uploadImage(this.selectedFile, "category").subscribe(result => {
      this.category.model.imageBucket = result.bucketName;
      this.category.model.imageFileName = result.fileName;
      this.imageService.getImage(this.category.model.imageBucket, this.category.model.imageFileName).then(image => {
        this.category.image = image;

      })
    })
  }
}
