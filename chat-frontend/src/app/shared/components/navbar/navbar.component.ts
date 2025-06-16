import { Component } from '@angular/core';
import { AuthHeaderComponent } from "../auth-header/auth-header.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [AuthHeaderComponent, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

}
