import {Component, } from '@angular/core';
import {ConfigService} from "../../../shared/services/config.service";
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent  {
  healthChecksURI: string = '';
  constructor(private config: ConfigService) {
    this.healthChecksURI = this.config.healthCheckURI;
  }
}
