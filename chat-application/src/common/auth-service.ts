import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { UserService } from "src/app/data/api/services";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(
    private userService: UserService,
    private router: Router) {}

  isLoggedIn(){
    if (localStorage.getItem('authenticateCredentials')) {
      // logged in so return true
      return true;
    }
    return false;
  }

  login(username: string, password: string, certificate: File){
    return this.userService.postApiV01UsersAuthenticateLoginForm({
      Username: username,
      Password: password,
      Certificate: certificate
    })
    .pipe(
      tap(otp => {
        localStorage.setItem('otp', JSON.stringify(otp));
        localStorage.setItem('userName', JSON.stringify(username));
      }),
      catchError(event => {
        this.removeLocalItems();
        return throwError(event);
      })
    );
  }

  logout() {
    
    return this.userService.postApiV01UsersRevokeToken()
    .pipe(
      tap(res => {
        this.removeLocalItems();
      }),
      catchError(event => {
        this.removeLocalItems();
        return throwError(event);
      })
    );
  }

  removeLocalItems() {
    localStorage.removeItem("authenticateCredentials");
    localStorage.removeItem("otp");
    localStorage.removeItem("userName");
  }

  otpApi(verificationCode: string){
    let otp = JSON.parse(localStorage.getItem('otp'));

    return this.userService.postApiV01UsersAuthenticateOtpOtpApiKey({
      otpApiKey: otp.otpApiKey,
      body: {
        otp: verificationCode
      }
    }).pipe(
      tap(authenticateCredentials => {
        localStorage.setItem('authenticateCredentials', JSON.stringify(authenticateCredentials));

      }),
      catchError(event => {
        return throwError(event);
      })
    );
  }

  refreshToken() {
    this.userService.postApiV01UsersRefreshToken()
    .subscribe(authenticateCredentials => {
      localStorage.setItem('authenticateCredentials', JSON.stringify(authenticateCredentials));
    });
  }

}
