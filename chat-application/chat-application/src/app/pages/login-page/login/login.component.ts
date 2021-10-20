import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/data/api/services';
import { AuthService } from 'src/common/auth-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  loginForm = this.formBuilder.group({
    username: '',
    password: '',
    file: ''
  });

  requestInProgress: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit(): void {
  }

  file: File

  submit() {
    this.requestInProgress = true;
    this.authService.login(this.loginForm.value.username, this.loginForm.value.password, this.file)
      .subscribe(otp => {
        this.requestInProgress = false;
        this.router.navigate(["verification-code"]);
      });
  }

  fileChange(event) {
    let fileList: FileList = event.target.files;
    if(fileList.length > 0) {
        this.file = fileList[0];
    }
  }

}
