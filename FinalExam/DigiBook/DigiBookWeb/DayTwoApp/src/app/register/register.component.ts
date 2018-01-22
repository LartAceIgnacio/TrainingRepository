import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../services/constants';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
    title: string;
    form: FormGroup;
    baseUrl: string = API_URL;

    constructor(private router: Router,
        private fb: FormBuilder,
        private http: HttpClient
    ) {
        this.title = 'New User Registration';
        // initialize the form
        this.createForm();
    }
    createForm() {
        this.form = this.fb.group({
            Username: ['', Validators.required],
            Email: ['',
                [Validators.required,
                Validators.email]],
            Password: ['', Validators.required],
            PasswordConfirm: ['', Validators.required],
            DisplayName: ['', Validators.required]
        }, {
                validator: this.passwordConfirmValidator
            });
    }

    onSubmit() {
        // build a temporary user object from form values
        // tslint:disable-next-line:prefer-const
        let tempUser = <User>{};
        tempUser.Username = this.form.value.Username;
        tempUser.Email = this.form.value.Email;
        tempUser.Password = this.form.value.Password;
        tempUser.DisplayName = this.form.value.DisplayName;
        // tslint:disable-next-line:prefer-const
        let url = `${this.baseUrl}/user`;
        this.http
            .post<User>(url, tempUser)
            .subscribe(res => {

                if (res) {
                    // tslint:disable-next-line:prefer-const
                    let v = res;
                    console.log('User ' + v.Username + ' has been created.');

                    // redirect to login page
                    this.router.navigate(['login']);
                } else {
                    // registration failed
                    this.form.setErrors({
                        'register': 'User registration failed.'
                    });
                }
            }, error => console.log(error));
    }

    onBack() {
        this.router.navigate(['/']);
    }


    passwordConfirmValidator(control: FormControl): any {
        // tslint:disable-next-line:prefer-const
        let p = control.root.get('Password');
        // tslint:disable-next-line:prefer-const
        let pc = control.root.get('PasswordConfirm');
        if (p && pc) {
            if (p.value !== pc.value) {
                pc.setErrors(
                    { 'PasswordMismatch': true }
                );
            } else {
                pc.setErrors(null);
            }
        }
        return null;
    }
    // retrieve a FormControl
    getFormControl(name: string) {
        return this.form.get(name);
    }
    // returns TRUE if the FormControl is valid
    isValid(name: string) {
        // tslint:disable-next-line:prefer-const
        let e = this.getFormControl(name);
        return e && e.valid;
    }
    // returns TRUE if the FormControl has been changed
    isChanged(name: string) {
        // tslint:disable-next-line:prefer-const
        let e = this.getFormControl(name);
        return e && (e.dirty || e.touched);
    }
    // returns TRUE if the FormControl is invalid after user changes
    hasError(name: string) {
        // tslint:disable-next-line:prefer-const
        let e = this.getFormControl(name);
        return e && (e.dirty || e.touched) && !e.valid;
    }
}
