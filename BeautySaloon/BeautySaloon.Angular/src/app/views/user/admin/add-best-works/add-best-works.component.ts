import { Component, HostListener } from '@angular/core';
import { SafeResourceUrl } from '@angular/platform-browser';
import { BestWorkModel } from 'src/app/shared/models/bestWork/best-work-model';
import { BestWorkWithImage } from 'src/app/shared/models/bestWork/best-work-with-image';
import { AuthService } from 'src/app/shared/services/auth.service';
import { BestWorkService } from 'src/app/shared/services/best-work.service';
import { ImageService } from 'src/app/shared/services/image.service';


@Component({
  selector: 'app-add-best-works',
  templateUrl: './add-best-works.component.html',
  styleUrls: ['./add-best-works.component.css']
})
export class AddBestWorksComponent {
  searchText: any;
  bestWorks: BestWorkWithImage[] = [];
  newBestWork: BestWorkWithImage = {
    model: {
      id: 0,
      imageBucket: '',
      imageFileName: ''
    },
    image: ''
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

  deleteBestWork(id: number) {

    this.bestWorkService.deleteBestWork(id).subscribe(result => {
      this.loadBestWorks();
    })

  }

  loadBestWorks() {
    this.bestWorkService.getBestWorks().subscribe(bestWorks => {
      const bestWorkPromises = bestWorks.map(element => {
        const bestWorkWithImage: BestWorkWithImage = {
          model: element,
          image: ''
        };

        const imagePromise = this.imageService.getImage(element.imageBucket, element.imageFileName);

        return Promise.all([imagePromise]).then(([image]) => {
          bestWorkWithImage.image = image;
          return bestWorkWithImage;
        });
      });

      Promise.all(bestWorkPromises).then(bestWorksWithImage => {
        this.bestWorks = bestWorksWithImage;
      });
    });
  }

  createBestWork() {
    this.bestWorkService.createBestWork(this.newBestWork.model).subscribe(result => {
      this.loadBestWorks(); // Обновляем список категорий после создания
      this.newBestWork = {
        model: {
          id: 0,
          imageBucket: '',
          imageFileName: ''
        },
        image: ''
      };
      this.showAddBestWorkForm = false;
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
    this.imageService.uploadImage(this.selectedFile, "best-works").subscribe(result => {
      this.newBestWork.model.imageBucket = result.bucketName;
      this.newBestWork.model.imageFileName = result.fileName;
      this.imageService.getImage(result.bucketName, result.fileName).then(image => {
        this.newBestWork.image = image;
      })
    })
  }

  handleMouseWheel(event: WheelEvent) {
    const container = document.querySelector('.bestWork-wrapper') as HTMLElement;
    if (container) {
      container.scrollLeft += event.deltaY;
      event.preventDefault(); // Остановим стандартную прокрутку страницы
    }
  }


}
