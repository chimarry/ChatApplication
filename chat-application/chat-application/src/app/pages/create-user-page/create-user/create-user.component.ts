import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/data/api/services';
import { AuthService } from 'src/common/auth-service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html'
})
export class CreateUserComponent implements OnInit {

  createUserForm = this.formBuilder.group({
    username: '',
    password: '',
    email: ''
  });

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit(): void {
  }

  submit() {
    this.userService.postApiV01Users({
      username: this.createUserForm.value.username,
      password: this.createUserForm.value.password,
      email: this.createUserForm.value.email
    }).subscribe(res => {
      this.router.navigate(["/"]);
    })
  }

}
