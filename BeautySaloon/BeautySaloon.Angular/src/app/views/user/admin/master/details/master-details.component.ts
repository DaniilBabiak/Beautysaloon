import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Master } from 'src/app/shared/models/master';
import { Service } from 'src/app/shared/models/service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { ServiceService } from 'src/app/shared/services/service.service';
import { ScheduleComponent } from '../schedule/schedule.component';

@Component({
  selector: 'app-details',
  templateUrl: './master-details.component.html',
  styleUrls: ['./master-details.component.css']
})
export class MasterDetailsComponent {
  @Input() masterId: number | undefined;
  isCloseAttempted: boolean = false;
  master: Master | null = null;
  unAssignedServices: Service[] | null = null;
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
    private auth: AuthService,
    private masterService: MasterService,
    private serviceService: ServiceService,
    private router: Router,
    private modalService: NgbModal) { }

  ngOnInit(): void {

  }

  showSchedule() {
    const modalRef = this.modalService.open(ScheduleComponent);
    modalRef.componentInstance.schedule = this.master?.schedule;
    modalRef.result.then(schedule => {
      if (this.master) {
        this.master.schedule = schedule;
      }
    })
  }

  loadMaster() {
    if (this.masterId == 0) {
      this.master = {
        id: 0,
        name: '',
        reservations: [],
        schedule: {
          id: 0,
          dayOffs: [],
          masterId: 0,
          master: this.master,
          workingDays: []
        },
        scheduleId: 0,
        services: []
      }

      this.loadUnAssignedServices();
    }
    else {
      this.auth.loadUser()?.then(() => {
        if (this.masterId) {
          this.masterService.getMaster(this.masterId).subscribe(result => {
            this.master = result;
            if (!this.master?.schedule) {
              this.master.schedule = {
                id: null,
                dayOffs: null,
                masterId: null,
                master: null,
                workingDays: []
              }
            }
            this.loadUnAssignedServices();

          });
        }
      });
    }


  }

  loadUnAssignedServices() {
    this.auth.loadUser()?.then(() => {
      this.serviceService.getServices().subscribe(result => {
        if (this.master) {
          this.unAssignedServices = result.filter(service =>
            !this.master?.services?.some(masterService => masterService.id === service.id)
          );
        }
      })
    })
  }

  saveDungeonMaster() {

    this.auth.loadUser()?.then(() => {
      console.log(this.master);
      if (this.master && this.master.id == 0) {
        this.masterService.createMaster(this.master).subscribe(result => {
          this.closeModal()
        });
      }
      else if (this.master && this.master.id != 0) {
        this.masterService.updateMaster(this.master).subscribe(result => {
          this.closeModal();
        })
      }
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  deleteMaster() {
    this.auth.loadUser()?.then(() => {
      if (this.master && this.master.id) {
        this.masterService.deleteMaster(this.master.id).subscribe(result => {
          this.closeModal();
        })
      }
    });
  }


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
  onDropAddService(event: any) {
    event.preventDefault();
    this.isServiceHovered = false;

    const serviceData = JSON.parse(event.dataTransfer.getData('text')); // Получение данных сервиса

    // Перемещение сервиса в выбранную категорию
    if (this.master?.services) {
      var alreadyInThisCategory = this.master.services?.findIndex(service => service.id === serviceData.id) != -1;
      if (!alreadyInThisCategory) {
        this.master.services?.push(serviceData);
      }


      // Удаление сервиса из списка без категории
      const index = this.unAssignedServices?.findIndex(service => service.id === serviceData.id) as number;
      if (index !== -1) {
        this.unAssignedServices?.splice(index, 1);
      }
    }
  }

  onDropRemoveService(event: any) {
    event.preventDefault();

    const serviceData = JSON.parse(event.dataTransfer.getData('text')) as Service; // Получение данных сервиса

    // Добавление сервиса в список сервисов без категории
    if (this.unAssignedServices) {
      var alreadyInThisCategory = this.unAssignedServices?.findIndex(service => service.id === serviceData.id) != -1;
      if (!alreadyInThisCategory) {
        this.unAssignedServices.push(serviceData);
      }
    }

    // Поиск и удаление сервиса из категорий
    if (this.master && this.master.services) {
      const serviceIndex = this.master.services.findIndex(service => service.id == serviceData.id);
      if (serviceIndex !== -1) {
        this.master.services?.splice(serviceIndex, 1);
      };
    }
  }
}
