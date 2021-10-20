import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html'
})
export class ErrorComponent implements OnInit {
  public message: string;
  public errorCode: number;
  constructor(private router: Router) {
    this.errorCode = this.getErrorCode();
    this.message = this.getErrorMessage();
   }

  ngOnInit(): void {
  }

  getErrorCode() {
    const state = this.router.getCurrentNavigation().extras.state;
    if (typeof (state) !== 'undefined' && state !== null) {
      return state.errorCode || 404;
    }
    return 404;
  }

  getErrorMessage() {
    const state = this.router.getCurrentNavigation().extras.state;

    if (typeof (state) === 'undefined' || state === null) {
      return 'Page not found.';
    }

    return this.getMessage(state.message);
  }

  getMessage(error: any) {
    var res = "";
    try {
      res = typeof error === "object" ? error.messages : JSON.parse(error).messages;
    } catch (e) {
      res =  error;
    }
    return res;
  }

}
