import { Component, OnInit } from '@angular/core';
import { TestRestService } from '../services/test-rest.service';

@Component({
  selector: 'app-test-rest',
  templateUrl: './test-rest.component.html',
  styleUrls: ['./test-rest.component.css']
})
export class TestRestComponent implements OnInit {
  values: string[] | null = null;

  constructor(private testService: TestRestService) {

  }

  ngOnInit(): void {
    this.testService.getWeatherForecast()
      .subscribe(result => {
        this.values = result;
      })
  }
}
