import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginPageModule } from './pages/login-page/login-page.module';
import { VerificationCodePageModule } from './pages/verification-code-page/verification-code-page.module';
import { ChatBrowserPageModule } from './pages/chat-browser-page/chat-browser-page.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiModule } from './data/api/api.module';
import { AuthGuard } from 'src/common/auth-guard';
import { JwtInterceptor } from 'src/common/jwt-interceptor';
import { UserService } from './data/api/services';
import { VerificationCodeGuard } from 'src/common/verification-code-guard';
import { ErrorPageModule } from './pages/error-page/error-page.module';
import { GlobalErrorHandler } from 'src/common/global-error-handler';
import { CreateUserPageModule } from './pages/create-user-page/create-user-page.module';
import { LoginGuard } from 'src/common/login-guard';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LoginPageModule,
    VerificationCodePageModule,
    ChatBrowserPageModule,
    HttpClientModule,
    ApiModule.forRoot({ rootUrl: "/api" }),
    ErrorPageModule,
    CreateUserPageModule,
  ],
  providers: [
    AuthGuard,
    VerificationCodeGuard,
    LoginGuard,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
