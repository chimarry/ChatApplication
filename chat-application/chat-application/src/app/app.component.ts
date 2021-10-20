import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/common/auth-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'chat-application';

  constructor(private authService: AuthService,
    private router: Router) {
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  logout() {
    this.authService.logout().subscribe(res => {
      this.router.navigate(['/login']);
    });
  }
}
