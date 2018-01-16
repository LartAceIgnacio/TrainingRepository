import { Component , OnInit} from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { invoke } from 'q';
import { HttpClient } from "@angular/common/http";
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { API_URL } from "../services/common/Authentication/constants";
import { RegistrationService } from "../services/common/Authentication/registration.service";

@Component({
  selector: "register",
  templateUrl: "./register.component.html",
  styleUrls: ['./register.component.css'],
  providers: [RegistrationService]
})
export class RegisterComponent {
  title: string;
  regForm: FormGroup;
  baseUrl: string = API_URL;

  constructor(private router: Router,
    private fb: FormBuilder,
    private http: HttpClient,
    private registrationService : RegistrationService
  ) {
    this.title = "New User Registration";
    // initialize the form
    this.createForm();
  }
  createForm() {
    this.regForm = this.fb.group({

      Username: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', Validators.required],
      PasswordConfirm: ['', Validators.required],
      DisplayName: ['', Validators.required],
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],

    }, {
        validator: this.passwordConfirmValidator
      }
    );
  }

  onSubmit() {
    // build a temporary user object from form values
    var tempUser = <User>{};
    tempUser.Username = this.regForm.value.Username;
    tempUser.Email = this.regForm.value.Email;
    tempUser.Password = this.regForm.value.Password;
    tempUser.DisplayName = this.regForm.value.DisplayName;
    tempUser.FirstName = this.regForm.value.FirstName;
    tempUser.LastName = this.regForm.value.LastName;

    this.registrationService.Register(tempUser, 'user')
    .then(res => {
      if (res) {
        var v = res;
        console.log("User " + v.Username + " has been created.");
        // redirect to login page
        this.router.navigate(["login"]);
      }
      else {
        // registration failed
        this.regForm.setErrors({
          "register": "User registration failed."
        });
      }
    });
    


    

    // var url = `${this.baseUrl}/user`;
    // this.http
    //   .post<User>(url, tempUser)
    //   .subscribe(res => {

    //     if (res) {
    //       var v = res;
    //       console.log("User " + v.Username + " has been created.");

    //       // redirect to login page
    //       this.router.navigate(["login"]);
    //     }
    //     else {
    //       // registration failed
    //       this.regForm.setErrors({
    //         "register": "User registration failed."
    //       });
    //     }
    //   }, error => console.log(error));
  }

  onBack() {
    this.router.navigate(["/dashboard"]);
  }


  passwordConfirmValidator(control: FormControl): any {
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
    return this.regForm.get(name);
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