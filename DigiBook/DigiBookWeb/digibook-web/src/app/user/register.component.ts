import { Component, Inject } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { API_URL} from '../services/constants';

@Component({
    selector: "register",
    templateUrl: "./register.component.html",
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
    title: string;
    form: FormGroup;

    baseUrl: string = API_URL;

    regexEmailFormat: string = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

    constructor(private router: Router,
        private fb: FormBuilder,
        private http: HttpClient
        ) {
        this.title = "New User Registration";
        // initialize the form
        this.createForm();
    }
    ngOnInit() {
        // this.loading = true;
        // setTimeout(() => {
        //   this.employeeService.getEmployees().then(employees => this.employeeList = employees);
        //   this.loading = false;
        // }, 1000);
        //this.selectedEmployee = this.employeeList[0];
    
        this.form = this.fb.group({
          'displayName': new FormControl('', Validators.required),
          'userName': new FormControl('', Validators.required),
          'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.pattern(this.regexEmailFormat)])),
          'password': new FormControl('', Validators.required),
          'confirmPassword': new FormControl('', Validators.required)
      });
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

    onSubmit() {
        
        // build a temporary user object from form values
        var tempUser = <User>{};
        tempUser.Username = this.form.value.userName;
        tempUser.Email = this.form.value.emailAddress;
        tempUser.Password = this.form.value.password;
        tempUser.DisplayName = this.form.value.displayName;
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