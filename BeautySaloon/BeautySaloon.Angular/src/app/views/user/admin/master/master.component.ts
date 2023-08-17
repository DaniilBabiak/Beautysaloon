import { Component, OnInit } from '@angular/core';
import { Master } from 'src/app/shared/models/master';
import { AuthService } from '../../../../shared/services/auth.service';
import { MasterService } from 'src/app/shared/services/master.service';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
  masters: Master[] | null = null;

  constructor(private auth: AuthService, private masterService: MasterService) { }
  ngOnInit(): void {
    this.auth.loadUser()?.then(() => {
      this.masterService.getAllMasters().subscribe(result => {
        console.log(result);
        this.masters = result;
        console.log(this.masters);
      })
    });
  }

}
