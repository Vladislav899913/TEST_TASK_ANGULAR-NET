import { Component, Input } from '@angular/core';
import { User } from 'src/app/models/user';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
})
export class FormComponent {
  public user!: User;
  phonePattern = /^((8|\+7)[\- ]?)?(\(?\d{3,4}\)?[\- ]?)?[\d\- ]{5,8}$/;

  constructor(private service: SharedService) {}

  // @Input() dep: any;

  ngOnInit() {
    this.user = {
      name: '',
      email: '',
      phone: '',
      theme: 'Техподдержка',
      message: '',
    };
  }

  recaptchaCallback() {
    document.getElementById('submit_button')!.removeAttribute('disabled');
  }

  onSubmit() {
    if (document.getElementById('form')?.classList.contains('ng-valid')) {
      var val = {
        name: this.user.name,
        email: this.user.email,
        phone: this.user.phone,
        theme: this.user.theme,
        message: this.user.message,
      };
      this.service.addData(val).subscribe((res) => {
        if (res) {
          alert('Сообщение успешно отправлено!');
          document.write(res);
        } else alert('Произошла ошибка!');
      });
    } else alert('Проверьте данные в полях формы!');
  }
}
