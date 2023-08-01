import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ChangePhoneNumberFormComponent } from '../change-phone-number-form/change-phone-number-form.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userName: string = '';
  phoneNumber: string = '';
  showChangePhoneNumberForm: boolean = false;
  constructor(private authService: AuthService) {

  }


  ngOnInit(): void {
    this.userName = this.authService.name;
    this.phoneNumber = this.authService.phoneNumber;
  }
  changePhoneNumber() {
    // Возможно, здесь вы захотите показать модальное окно или перенаправить пользователя на отдельную страницу для изменения номера телефона.
    // В этом случае, вы можете использовать Angular Router или модальные окна библиотеки (например, Angular Material Dialog).

    // Но, если вы хотите показать форму на этой же странице, вы можете добавить флаг, который покажет форму в шаблоне:
    this.showChangePhoneNumberForm = !this.showChangePhoneNumberForm;
  }
}
