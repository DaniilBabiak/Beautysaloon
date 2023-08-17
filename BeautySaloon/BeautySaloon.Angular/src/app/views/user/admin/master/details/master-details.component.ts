import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Master } from 'src/app/shared/models/master';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';

@Component({
  selector: 'app-details',
  templateUrl: './master-details.component.html',
  styleUrls: ['./master-details.component.css']
})
export class MasterDetailsComponent {
  masterId: number | undefined;
  master: Master | null = null;

  constructor(private route: ActivatedRoute, private auth: AuthService, private masterService: MasterService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.masterId = +params['id'];
      this.loadMaster();
    });
  }

  loadMaster() {
    this.auth.loadUser()?.then(() => {
      if (this.masterId) {
        this.masterService.getMaster(this.masterId).subscribe(result => {
          this.master = result;
        });
      }
    });
  }
}
