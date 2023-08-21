import { Component, OnInit } from '@angular/core';
import { Master } from 'src/app/shared/models/master';
import { AuthService } from '../../../../shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MasterDetailsComponent } from './details/master-details.component';
import { ScheduleComponent } from './schedule/schedule.component';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
  masters: Master[] | null = null;

  constructor(private auth: AuthService, private masterService: MasterService, private modalService: NgbModal) { }
  ngOnInit(): void {
    this.loadMasters();
  }

  loadMasters() {
    this.auth.loadUser()?.then(() => {
      this.masterService.getAllMasters().subscribe(result => {
        this.masters = result;
      })
    });
  }

  openMasterDetails(master: Master) {
    const modalRef = this.modalService.open(MasterDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.masterId = master.id;
    modalRef.componentInstance.loadMaster();

    modalRef.closed.subscribe(result => {
      console.log(result);
      this.loadMasters();
    })
  }

  createMaster() {
    const modalRef = this.modalService.open(MasterDetailsComponent, { backdrop: 'static' });
    modalRef.componentInstance.masterId = 0;
    modalRef.componentInstance.loadMaster();

    modalRef.closed.subscribe(result => {
      console.log(result);
      this.loadMasters();
    })
  }

  configureSchedule(master: Master) {
    const modalRef = this.modalService.open(ScheduleComponent, { backdrop: 'static' });
    modalRef.componentInstance.masterId = master.id;
    modalRef.componentInstance.loadSchedule();
    modalRef.closed.subscribe(() => {
      this.loadMasters();
    });
  }

}
