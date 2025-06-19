import { FormControl, FormGroup, Validators } from "@angular/forms";

interface FormModel {
    email: FormControl<string>,
    password: FormControl<string>
}

export const form: FormGroup<FormModel> = new FormGroup<FormModel>({
    email: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.required, Validators.email]
    }),
    password: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[_-])[A-Za-z\\d_-]{8,}$')]
    })
})