import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  isAuthenticated: boolean = false;
  userName: string | undefined;
  isAdmin: boolean = false;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()){
      this.isAuthenticated = true;
      this.userName = this.authService.name
      this.isAdmin = this.authService.isAdmin();
    }
  }

  login() {
    this.authService.login();
    this.userName = this.authService.name;
  }

  async signout(){
    await this.authService.signout();
  }
}
