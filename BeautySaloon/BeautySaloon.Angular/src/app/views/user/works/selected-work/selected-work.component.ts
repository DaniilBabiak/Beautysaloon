import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BestWorkWithImage } from 'src/app/shared/models/bestWork/best-work-with-image';
import { AuthService } from 'src/app/shared/services/auth.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { ImageService } from 'src/app/shared/services/image.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { ServiceService } from 'src/app/shared/services/service.service';

@Component({
  selector: 'app-selected-work',
  templateUrl: './selected-work.component.html',
  styleUrls: ['./selected-work.component.css']
})
export class SelectedWorkComponent {
  bestWorks: BestWorkWithImage[] = [];
  selectedWork: BestWorkWithImage | null = null;
  isAdmin: boolean = false;

  constructor(
    public activeModal: NgbActiveModal) {
  }

  initModal(bestWorks: BestWorkWithImage[], selectedBestWorkId: number, isAdmin: boolean) {
    this.bestWorks = bestWorks;
    this.selectedWork = this.bestWorks.find(bestWork => bestWork.model.id === selectedBestWorkId) as BestWorkWithImage;
    this.isAdmin = isAdmin;
    console.log(this.selectedWork);
  }

  closeModal() {
    this.activeModal.close();
  }
}
