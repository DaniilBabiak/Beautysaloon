import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterDetailsComponent } from './details/master-details.component';
import { MasterModel } from 'src/app/shared/models/master/master-model';
import { MasterDetailedModel } from 'src/app/shared/models/master/master-detailed-model';
import { ScheduleComponent } from './schedule/schedule.component';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
  masters: MasterModel[] | null = null;

  constructor(private auth: AuthService, private masterService: MasterService, private modalService: NgbModal) { }
  ngOnInit(): void {
    this.loadMasters();
  }

  loadMasters() {
    this.auth.loadUser()?.then(() => {
      this.masterService.getAllMasters().subscribe(result => {
        this.masters = result;
        console.log(result);
      })
    });
  }

  openMasterDetails(master: MasterModel) {
    const modalRef = this.modalService.open(MasterDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.masterId = master.id;
    modalRef.componentInstance.initModal();

    modalRef.closed.subscribe(result => {
      this.loadMasters();
    })
  }

  createMaster() {
    const modalRef = this.modalService.open(MasterDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.masterId = 0;
    modalRef.componentInstance.initModal();

    modalRef.closed.subscribe(result => {
      this.loadMasters();
    })
  }

  configureSchedule(master: MasterModel) {
    if (master.scheduleId == null || master.scheduleId == 0){
      const modalRef = this.modalService.open(ScheduleComponent, { backdrop: 'static' });
      modalRef.componentInstance.masterId = master.id;
      modalRef.componentInstance.loadSchedule();
      modalRef.closed.subscribe(() => {
        this.loadMasters();
      });
    }
  }

}
