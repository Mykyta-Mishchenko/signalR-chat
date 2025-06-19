import { AbstractControl, FormControl, FormGroup, Validators } from "@angular/forms";

interface FormModel {
  userName: FormControl<string>;
  email: FormControl<string>;
  passwords: FormGroup<{
    password: FormControl<string>;
    confirmedPassword: FormControl<string>;
  }>;
}

export const form: FormGroup<FormModel> = new FormGroup<FormModel>({
  userName: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.pattern('^[a-zA-Z][a-zA-Z0-9_]{3,}$')]
  }),
  email: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email]
    }),
  passwords: new FormGroup({
    password: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_-])[A-Za-z\\d_-]{8,}$')]
    }),
    confirmedPassword: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_-])[A-Za-z\\d_-]{8,}$')]
    })
  },
  {
    validators: [equalPasswords]
  })
})
function equalPasswords(control: AbstractControl) {
    const password = control.get('password')?.value;
    const confirmedPassword = control.get('confirmedPassword')?.value;
  
    if (password === confirmedPassword && password) {
      return null;
    }
  
    return { passwordsNotEqual: true };
}