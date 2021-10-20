import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/common/auth-guard';
import { LoginGuard } from 'src/common/login-guard';
import { VerificationCodeGuard } from 'src/common/verification-code-guard';
import { ChatBrowserComponent } from './pages/chat-browser-page/chat-browser/chat-browser.component';
import { CreateUserComponent } from './pages/create-user-page/create-user/create-user.component';
import { ErrorComponent } from './pages/error-page/error/error.component';
import { LoginComponent } from './pages/login-page/login/login.component';
import { VerificationCodeComponent } from './pages/verification-code-page/verification-code/verification-code.component';


const routes: Routes = [
  {
    path: "",
    component: LoginComponent,
    data: {
      title: "login",
      breadcrumb: "login",
    },
    // canActivate: [LoginGuard] 
  },
  {
    path: "login",
    component: LoginComponent,
    data: {
      title: "login",
      breadcrumb: "login",
    },
    // canActivate: [LoginGuard] 
  },
  {
    path: "create-user",
    component: CreateUserComponent,
    data: {
      title: "create-user",
      breadcrumb: "create-user",
    },
    canActivate: [LoginGuard] 
  },
  {
    path: "error",
    component: ErrorComponent,
    data: {
      title: "error",
      breadcrumb: "error",
    },
  },
  {
    path: "verification-code",
    component: VerificationCodeComponent,
    data: {
      title: "verification-code",
      breadcrumb: "verification-code",
    },
    canActivate: [VerificationCodeGuard] 
  },
  {
    path: "chat-browser",
    component: ChatBrowserComponent,
    data: {
      title: "chat-browser",
      breadcrumb: "chat-browser",
    },
    canActivate: [AuthGuard] 
  },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
