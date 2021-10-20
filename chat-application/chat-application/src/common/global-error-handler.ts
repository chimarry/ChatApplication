import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from './auth-service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(
    private injector: Injector,
    private router: Router,
    private zone: NgZone,
    private authService: AuthService) { }

  handleError(response: Error | HttpErrorResponse) {
    let message;
    let status;
    if (response instanceof HttpErrorResponse) {
      // Server error
      if( response.error?.message != undefined) {
        message = response.error.message;
      } else if( response.error?.error != undefined) {
        message = response.error.error;
      } else {
        message = response.error;
      }

      status = response.status;
    } else {
      // Client Error
      message = "Page not found.";
      status = 404;
    }
    if(status === 401 || status === 500 || status === 400){
        this.authService.logout()
        .subscribe();
    }
    // Always log errors
    this.navigeteToErrorPage(message, status);
  }

  navigeteToErrorPage(errorMessage: string, code: number): void {
    this.zone.run(() => {
      this.router.navigate(['/error'], { state: { message: errorMessage, errorCode: code } });
    });
  }
}
