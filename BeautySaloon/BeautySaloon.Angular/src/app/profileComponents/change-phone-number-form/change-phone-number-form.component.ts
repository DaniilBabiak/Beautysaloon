import { Component } from '@angular/core';

@Component({
  selector: 'app-change-phone-number-form',
  templateUrl: './change-phone-number-form.component.html',
  styleUrls: ['./change-phone-number-form.component.css']
})
export class ChangePhoneNumberFormComponent {
  phoneNumber: string | null = ''; // Здесь предполагается, что у вас уже есть значение phoneNumber
  newPhoneNumber: string = '';

  changePhoneNumber() {
    // Проверка на null или пустую строку
    if (!this.newPhoneNumber.trim()) {
      return;
    }

    // Обновление значения phoneNumber
    this.phoneNumber = this.newPhoneNumber;
    this.newPhoneNumber = ''; // Очистка поля ввода после изменения номера
  }
}
