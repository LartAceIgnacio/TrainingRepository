import { Component, Inject } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { API_URL } from '../services/constants';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {
  title: string;
  form: FormGroup;
  baseUrl: string = API_URL;
  rexExpEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
  rexExpPasswordFormat: string = "((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{8,20})";

  constructor(private router: Router, private fb: FormBuilder, private http: HttpClient ) {
    this.title = "New User Registration";
    // initialize the form
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern(this.rexExpEmailFormat)]],
      Password: ['', [Validators.required]],
      confirmPassword: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    }, 
    {
      validator: this.passwordConfirmValidator,
      //validator: this.passwordValidator
    });
  }

  onSubmit() {
    // build a temporary user object from form values
    var tempUser = <User>{};
    tempUser.Username = this.form.value.userName;
    tempUser.Email = this.form.value.email;
    tempUser.Password = this.form.value.confirmPassword;
    tempUser.DisplayName = `${this.form.value.firstName} ${this.form.value.lastName}`;
    var url = `${this.baseUrl}/user`;
    this.http
      .post<User>(url, tempUser)
      .subscribe(res => {
        if (res) {
          var v = res;
          console.log("User " + v.Username + " has been created.");

          // redirect to login page
          this.router.navigate(["login"]);
        }
        else {
          // registration failed
          this.form.setErrors({
            "register": "User registration failed."
          });
        }
      }, error => console.log(error));
  }

  onBack() {
    this.router.navigate(["/"]);
  }


  passwordConfirmValidator(control: FormControl): any {
    let p = control.root.get('Password');
    let pc = control.root.get('confirmPassword');
    let upperCase = /[A-Z]+/;
    let symbols = /[-!$%^&*()_+|~=`{}\[\]:";'<>?,.\/]/;

    if (p && pc) {
      if (p.value !== pc.value && pc.touched) {
        pc.setErrors(
          { "PasswordMismatch": true }
        );
      } 
      else if(!upperCase.test(p.value) || !symbols.test(p.value)) {
        p.setErrors(
          // { "Password must be contain at least 1 upper case character.": true }
          { "UpperCase" : true }
        );
      }
      else {
        p.setErrors(null);
      }
    }
    return null;
  }
}
