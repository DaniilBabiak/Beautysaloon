import { Component } from '@angular/core';
import { BestWork } from 'src/app/shared/models/best-work';
import { AuthService } from 'src/app/shared/services/auth.service';
import { BestWorkService } from 'src/app/shared/services/best-work.service';
import { ImageService } from 'src/app/shared/services/image.service';

@Component({
  selector: 'app-add-best-works',
  templateUrl: './add-best-works.component.html',
  styleUrls: ['./add-best-works.component.css']
})
export class AddBestWorksComponent {
  bestWorks: BestWork[] | null = null;
  newBestWork: BestWork = {
    id: null,
    imageBucket: null,
    imageFileName: null,
    image: null
  };

  showAddBestWorkForm = false;
  selectedFile: File | null = null;

  constructor(private bestWorkService: BestWorkService, private imageService: ImageService, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.authService.loadUser()?.then(() => {
      this.loadBestWorks();
    })
  }

  deleteBestWork(id: number | null) {
    if (id) {
      this.bestWorkService.deleteBestWork(id).subscribe(result => {
        this.loadBestWorks();
      })
    }

  }

  loadBestWorks() {
    this.bestWorkService.getBestWorks().subscribe(result => {
      this.bestWorks = result;
      this.loadImages();
    });
  }

  loadImages() {
    this.bestWorks?.forEach(element => {
      if (element.imageBucket && element.imageFileName) {
        this.imageService.getImage(element.imageBucket, element.imageFileName).then(data => {
          element.image = data;

        });
      }
    });
  }

  createBestWork() {
    this.bestWorkService.createBestWork(this.newBestWork).subscribe(result => {
      this.loadBestWorks(); // Обновляем список категорий после создания
      this.newBestWork = {
        id: null,
        imageBucket: null,
        imageFileName: null,
        image: null
      };
      this.showAddBestWorkForm = false;
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
    this.imageService.uploadImage(this.selectedFile, "categories").subscribe(result => {
      this.newBestWork.imageBucket = result.bucketName;
      this.newBestWork.imageFileName = result.fileName;
      if (this.newBestWork.imageBucket && this.newBestWork.imageFileName) {
        this.imageService.getImage(this.newBestWork.imageBucket, this.newBestWork.imageFileName).then(data => {
          this.newBestWork.image = data;
        })
      }

    })
  }
}
