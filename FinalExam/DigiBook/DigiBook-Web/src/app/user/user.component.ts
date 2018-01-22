import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../services/constants';
import {Message} from 'primeng/primeng';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  title: string;
  form: FormGroup;
  baseUrl: string = API_URL;
  msgs: Message[] = [];
  
  constructor(private router: Router, private fb: FormBuilder, private http: HttpClient) { 
      this.title = "New User Registration";
        // initialize the form
        this.createForm();
    }
    createForm() {
      this.form = this.fb.group({
          Username: ['', Validators.required],
          Email: ['',
              [Validators.required,
              Validators.email] ],
             Password: ['', Validators.required],
             PasswordConfirm: ['', Validators.required],
             DisplayName: ['', Validators.required]
        }, {
          validator: this.passwordConfirmValidator
    }); 
  }

  ngOnInit() {
  }
  onSubmit() {
    // build a temporary user object from form values
    var tempUser = <User>{};
    tempUser.Username = this.form.value.Username;
    tempUser.Email = this.form.value.Email;
    tempUser.Password = this.form.value.Password;
    tempUser.DisplayName = this.form.value.DisplayName;

    var url = `${this.baseUrl}/User`;
    this.http
        .post<User>(url, tempUser)
        .subscribe(res => {
    
    if (res) {
        var v = res;
      
        this.msgs.push({severity:'info', summary:'Register Sucess', detail:'Success'});
        console.log("User " + v.Username + " has been created.");
        setTimeout(() => {
           
        }, 3000);
        // redirect to login page
        this.router.navigate(["login"]);  
    }
    else {
        // registration failed
        this.form.setErrors({
                "register": "User registration failed."
            });
        }
    },
     error => console.log(error));

  }

onBack() {
    this.router.navigate(["/"]);
}


passwordConfirmValidator(control: FormControl):any {
  let p = control.root.get('Password');
  let pc = control.root.get('PasswordConfirm');
  if (p && pc) {
      if (p.value !== pc.value) {
          pc.setErrors(
              { "PasswordMismatch": true }
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
    var e = this.getFormControl(name);
    return e && e.valid;
}
// returns TRUE if the FormControl has been changed
isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched);
}
// returns TRUE if the FormControl is invalid after user changes
hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
}

}
