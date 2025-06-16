import { Component, inject, OnInit, Signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { User } from '../../../core/services/auth/models/user.models';

@Component({
  selector: 'app-auth-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './auth-header.component.html',
  styleUrl: './auth-header.component.css'
})
export class AuthHeaderComponent implements OnInit{
  private authService = inject(AuthService);
  private router = inject(Router);

  user!: Signal<User | null>;
  isLoggedIn!: Signal<boolean>;
  ngOnInit(): void {
    this.user = this.authService.User;
    this.isLoggedIn = this.authService.isLoggedIn;
    
  }

  onLogout() {
    this.authService.logout().subscribe({
      error: () => {
        this.router.navigate(['auth/signIn']);
      }
    });
  }
}
