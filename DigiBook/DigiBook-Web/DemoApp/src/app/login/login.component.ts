import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { API_URL } from '../services/constants';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  title: string;
  form: FormGroup;
  baseUrl: string = API_URL;

  
  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService) {
    this.title = "User Login";

      this.createForm();
   }
   createForm() {
    this.form = this.fb.group({
        Username: ['', Validators.required],
        Password: ['', Validators.required]
    });
  }
  ngOnInit() {
  }

  onSubmit() {
    var url = `${this.baseUrl}/token/auth`;
    var username = this.form.value.Username;
    var password = this.form.value.Password;
    this.authService.login(username, password)
        .subscribe(res => {
                // login successful
                // outputs the login info through a JS alert.
                // IMPORTANT: remove this when test is done.
                /*alert("Login successful! "
                    + "USERNAME: "
                    + username
                    + " TOKEN: "
                    + this.authService.getAuth()!.token
                );
                */
                this.router.navigate(["/"]);
            },
            err => {
                // login failed
                console.log("royroyroy")
                this.form.setErrors({
                    "auth": "Incorrect username or password"
                });
                
          });
    }
    onBack() {
      this.router.navigate(["/"]);
    }
    getFormControl(name: string) {
      return this.form.get(name);
    }
    isValid(name: string) {
      var e = this.getFormControl(name);
      return e && e.valid;
    }
    isChanged(name: string) {
      var e = this.getFormControl(name);
      return e && (e.dirty || e.touched);
    }
    hasError(name: string) {
      var e = this.getFormControl(name);
      return e && (e.dirty || e.touched) && !e.valid;
    }

}
