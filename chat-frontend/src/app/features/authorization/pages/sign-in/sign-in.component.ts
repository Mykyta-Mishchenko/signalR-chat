import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { form as reactiveForm} from './sign-in.reactive-form';
import { ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { debounceTime } from 'rxjs';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth/auth.service';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterLink],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css'
})
export class SignInComponent implements OnInit{
  form = reactiveForm;
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);
  isAuthorizationSucceeded = true;

  ngOnInit() {

    const savedForm = window.localStorage.getItem('saved-sign-in-form');

    if (savedForm) {
      const loadedForm = JSON.parse(savedForm);
      const currentTime = new Date().getTime();
      const expirationDuration = 1000 * 60 * 60; //1 hour 
      const formIsRelevant = currentTime - loadedForm.expiry < expirationDuration;
      
      if (formIsRelevant) {
        this.form.patchValue({
          email: loadedForm.email
        })
        if (loadedForm.email !== '') {
          this.form.controls.email.markAsTouched(); 
        } 
      }
    }

    const subscriprion = this.form.valueChanges.pipe(debounceTime(500)).subscribe({
      next: value => {
        window.localStorage.setItem(
          'saved-sign-in-form',
          JSON.stringify({ email: value.email, expiry: new Date().getTime() }));
      }
    })

    this.destroyRef.onDestroy(() => subscriprion.unsubscribe());
  }

  get emailIsValid() {
    return (
      this.form.controls.email.touched &&
      this.form.controls.email.valid
    )
  }

  get passwordIsValid() {
    return (
      this.form.controls.password.touched &&
      this.form.controls.password.valid
    )
  }

  onSubmit() {
    if (this.emailIsValid && this.passwordIsValid) {
      this.authService.signIn({
        email: this.form.controls.email.value,
        password: this.form.controls.password.value
      }).subscribe({
        next: (response) => {
          this.router.navigate(['']);
        },
        error: (response) => {
          this.isAuthorizationSucceeded = false;
          console.log("Sign in was failed.");
          console.log(response);
        }
      });
    }
  }
}
