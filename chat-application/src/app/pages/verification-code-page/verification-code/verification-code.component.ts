import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/data/api/services';
import { AuthService } from 'src/common/auth-service';

@Component({
  selector: 'app-verification-code',
  templateUrl: './verification-code.component.html'
})
export class VerificationCodeComponent implements OnInit {

  verificationForm = this.formBuilder.group({
    verificationCode: ''
  });
  requestInProgress: boolean = false;
  constructor(private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {

  }

  submit() {
    this.requestInProgress = true;
    this.authService.otpApi(this.verificationForm.value.verificationCode)
      .subscribe(otp => {
        this.requestInProgress = false;
        this.router.navigate(["chat-browser"]);
      });
  }

}
