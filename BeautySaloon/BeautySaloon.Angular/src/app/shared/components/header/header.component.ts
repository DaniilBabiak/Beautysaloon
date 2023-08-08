import { Component, OnInit } from '@angular/core';



@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  isAuthenticated: boolean = false;
  userName: string | undefined;
  isAdmin: boolean = false;
  constructor() { }

  ngOnInit(): void {
  }
}
