import { Component, DestroyRef, inject, OnInit} from '@angular/core';
import { ReactiveFormsModule} from '@angular/forms'
import { form as reactiveForm } from './sign-up.reactive-form';
import { debounceTime } from 'rxjs';
import { NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth/auth.service';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterLink],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent implements OnInit{

  private router = inject(Router);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  form = reactiveForm;

  ngOnInit() {
    const savedForm = window.localStorage.getItem('saved-sign-up-form')

    if (savedForm) {
      const loadedForm = JSON.parse(savedForm);
      const currentTime = new Date().getTime();
      const expirationDuration = 1000 * 60 * 60; //1 hour 
      const formIsRelevant = currentTime - loadedForm.expiry < expirationDuration;
      
      if (formIsRelevant) {
        this.form.patchValue({
          userName: loadedForm.userName,
          email: loadedForm.email
        })
        if (loadedForm.firstName !== '') {
          this.form.controls.userName.markAsTouched(); 
        }
        if (loadedForm.email !== '') {
          this.form.controls.email.markAsTouched(); 
        } 
      }
    }

    const subscription = this.form.valueChanges.pipe(debounceTime(500)).subscribe({
      next: value => {
        window.localStorage.setItem(
          'saved-sign-up-form',
          JSON.stringify({
            userName: value.userName,
            email: value.email,
            expiry: new Date().getTime()
          }));
      }
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  get userNameIsValid() {
    return (
      this.form.controls.userName.touched &&
      this.form.controls.userName.valid
    )
  }

  get emailIsValid() {
    return (
      this.form.controls.email.touched&&
      this.form.controls.email.valid
    )
  }

  get passwordIsValid() {
    return (
      this.form.controls.passwords.get('password')?.touched &&
      this.form.controls.passwords.get('password')?.valid
    )
  }

  get confirmedPasswordIsValid() {
    return (
      this.form.controls.passwords.get('confirmedPassword')?.touched &&
      this.form.controls.passwords.get('confirmedPassword')?.valid &&
      this.form.controls.passwords.valid
    )
  }

  onSubmit() {
    if (
      this.userNameIsValid &&
      this.emailIsValid &&
      this.passwordIsValid &&
      this.confirmedPasswordIsValid
    ) {
      
      this.authService.signUp({
        userName: this.form.controls.userName.value,
        email: this.form.controls.email.value,
        password: this.form.controls.passwords.controls.password.value,
        confirmedPassword: this.form.controls.passwords.controls.confirmedPassword.value,
      }).subscribe({
        next: (response) => {
          this.router.navigate(['/auth/signIn']);
        },
        error: (response) => {
          console.log("Sign up was failed.");
          console.log(response);
        }
      })
    }
  }
}
